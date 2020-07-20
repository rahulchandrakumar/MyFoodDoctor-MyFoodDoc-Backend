using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using MyFoodDoc.App.Application.Abstractions;
using MyFoodDoc.App.Application.Configuration;
using MyFoodDoc.App.Application.Models;
using MyFoodDoc.App.Application.Payloads.Target;
using MyFoodDoc.Application.Abstractions;
using MyFoodDoc.Application.Entities;
using MyFoodDoc.Application.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MyFoodDoc.App.Application.Exceptions;
using MyFoodDoc.Application.Entities.Targets;

namespace MyFoodDoc.App.Application.Services
{
    public class TargetService : ITargetService
    {
        private readonly IApplicationContext _context;
        private readonly IFoodService _foodService;
        private readonly IDiaryService _diaryService;
        private readonly int _statisticsPeriod;

        public TargetService(IApplicationContext context, IFoodService foodService, IDiaryService diaryService, IOptions<StatisticsOptions> statisticsOptions)
        {
            _context = context;
            _foodService = foodService;
            _diaryService = diaryService;
            _statisticsPeriod = statisticsOptions.Value.Period > 0 ? statisticsOptions.Value.Period : 7;
        }

        public async Task<ICollection<OptimizationAreaDto>> GetAsync(string userId, CancellationToken cancellationToken)
        {
            var result = new List<OptimizationAreaDto>();

            List<UserTarget> userTargets = new List<UserTarget>();

            var userTargetsForStatisticsPeriod = await _context.UserTargets.Where(x =>
                x.UserId == userId && x.Created > DateTime.Now.AddDays(-_statisticsPeriod)).ToListAsync(cancellationToken);

            if (userTargetsForStatisticsPeriod.Any())
            {
                userTargets = userTargetsForStatisticsPeriod
                    .GroupBy(g => g.TargetId).Select(x => x.OrderBy(y => y.Created).Last()).ToList();
            }
            else
            {
                if (!await _diaryService.IsDiaryFull(userId, cancellationToken))
                    return result;

                var userDiets = await _context.UserDiets.Where(x => x.UserId == userId).Select(x => x.DietId).ToListAsync(cancellationToken);
                var userIndications = await _context.UserIndications.Where(x => x.UserId == userId).Select(x => x.IndicationId).ToListAsync(cancellationToken);
                var userMotivations = await _context.UserMotivations.Where(x => x.UserId == userId).Select(x => x.MotivationId).ToListAsync(cancellationToken);

                var dietTargets = userDiets.Any() ? await _context.DietTargets.Where(x => userDiets.Contains(x.DietId)).Select(x => x.TargetId).ToListAsync(cancellationToken) : new List<int>();
                var indicationTargets = userIndications.Any() ? await _context.IndicationTargets.Where(x => userIndications.Contains(x.IndicationId)).Select(x => x.TargetId).ToListAsync(cancellationToken) : new List<int>();
                var motivationTargets = userMotivations.Any() ? await _context.MotivationTargets.Where(x => userMotivations.Contains(x.MotivationId)).Select(x => x.TargetId).ToListAsync(cancellationToken) : new List<int>();

                var targetIds = dietTargets
                    .Union(indicationTargets)
                    .Union(motivationTargets).Distinct().ToList();
                
                if (!targetIds.Any())
                    return result;

                var dailyUserIngredients = await GetDailyUserIngredients(userId, DateTime.Now, cancellationToken);

                foreach (var dailyMeals in _context.Meals
                    .Where(x => x.UserId == userId && x.Date > DateTime.Now.AddDays(-_statisticsPeriod)).ToList().GroupBy(g => g.Date))
                {
                    var dailyNutritions = new MealNutritionsDto
                    {
                        AnimalProtein = 0,
                        PlantProtein = 0,
                        Sugar = 0,
                        Vegetables = 0
                    };

                    foreach (var meal in dailyMeals)
                    {
                        var mealNutritions = await _foodService.GetMealNutritionsAsync(meal.Id, cancellationToken);

                        dailyNutritions.AnimalProtein += mealNutritions.AnimalProtein;
                        dailyNutritions.PlantProtein += mealNutritions.PlantProtein;
                        dailyNutritions.Sugar += mealNutritions.Sugar;
                        dailyNutritions.Vegetables += mealNutritions.Vegetables;
                    }

                    dailyUserIngredients[dailyMeals.Key] = dailyNutritions;
                }

                if (!dailyUserIngredients.Any())
                    return result;

                foreach (var target in _context.Targets.Include(x => x.OptimizationArea).Where(x => targetIds.Contains(x.Id)).OrderBy(x => x.Priority))
                {
                    int triggeredDaysCount = 0;

                    if (target.OptimizationArea.Type == OptimizationAreaType.Protein)
                    {
                        var weightHistory = await _context.UserWeights
                            .Where(x => x.UserId == userId && x.Date > DateTime.Now.AddDays(-_statisticsPeriod)).ToListAsync(cancellationToken);

                        decimal weight = 0;

                        if (weightHistory.Any())
                        {
                            weight = weightHistory.Average(x => x.Value);
                        }
                        else
                        {
                            var userWeight = _context.UserWeights.Where(x => x.Date < DateTime.Now.AddDays(-_statisticsPeriod))
                                .OrderBy(x => x.Date).LastOrDefault();

                            if (userWeight == null)
                            {
                                continue;
                            }
                            else
                            {
                                weight = userWeight.Value;
                            }
                        }

                        if (target.TriggerOperator == TriggerOperator.GreaterThan)
                        {
                            var triggeredDays = dailyUserIngredients.Values.Where(x => x.Protein / weight > target.TriggerValue);

                            triggeredDaysCount = triggeredDays.Count();
                        }
                        else
                        {
                            var triggeredDays = dailyUserIngredients.Values.Where(x => x.Protein / weight < target.TriggerValue);

                            triggeredDaysCount = triggeredDays.Count();
                        }
                    }
                    else if (target.OptimizationArea.Type == OptimizationAreaType.Sugar)
                    {
                        if (target.TriggerOperator == TriggerOperator.GreaterThan)
                        {
                            var triggeredDays = dailyUserIngredients.Values.Where(x => x.Sugar > target.TriggerValue);

                            triggeredDaysCount = triggeredDays.Count();
                        }
                        else
                        {
                            var triggeredDays = dailyUserIngredients.Values.Where(x => x.Sugar < target.TriggerValue);

                            triggeredDaysCount = triggeredDays.Count();
                        }
                    }
                    else if (target.OptimizationArea.Type == OptimizationAreaType.Vegetables)
                    {
                        if (target.TriggerOperator == TriggerOperator.GreaterThan)
                        {
                            var triggeredDays = dailyUserIngredients.Values.Where(x => x.Vegetables > target.TriggerValue);

                            triggeredDaysCount = triggeredDays.Count();
                        }
                        else
                        {
                            var triggeredDays = dailyUserIngredients.Values.Where(x => x.Vegetables < target.TriggerValue);

                            triggeredDaysCount = triggeredDays.Count();
                        }
                    }

                    var frequency = (decimal)triggeredDaysCount * 100 / _statisticsPeriod;

                    if (frequency > target.Threshold)
                    {
                        var userTarget = new UserTarget { UserId = userId, TargetId = target.Id, Created = DateTime.Now};

                        userTargets.Add(userTarget);
                    }
                }

                await _context.SaveChangesAsync(cancellationToken);
            }

            foreach (var userTarget in userTargets)
            {
                var target = await _context.Targets.Include(x => x.OptimizationArea)
                                                    .Include(x => x.Image)
                                                    .SingleAsync(x => x.Id == userTarget.TargetId, cancellationToken);

                var targetDto = new TargetDto
                {
                    Id = target.Id,
                    Type = target.Type.ToString(),
                    Title = target.Title,
                    Text = target.Text,
                    ImageUrl = target.Image.Url
                };

                var dailyUserIngredients = await GetDailyUserIngredients(userId, userTarget.Created, cancellationToken);

                if (target.Type == TargetType.Adjustment)
                {
                    var adjustmentTarget = await _context.AdjustmentTargets.SingleAsync(x => x.TargetId == target.Id, cancellationToken);

                    decimal bestValue = 0;

                    if (target.OptimizationArea.Type == OptimizationAreaType.Protein)
                    {
                        var weightHistory = await _context.UserWeights
                            .Where(x => x.UserId == userId && x.Date > userTarget.Created.AddDays(-_statisticsPeriod) && x.Date < userTarget.Created).ToListAsync(cancellationToken);

                        decimal weight = 0;

                        if (weightHistory.Any())
                        {
                            weight = weightHistory.Average(x => x.Value);
                        }
                        else
                        {
                            var userWeight = await _context.UserWeights.Where(x => x.Date < userTarget.Created.AddDays(-_statisticsPeriod))
                                .OrderBy(x => x.Date).LastOrDefaultAsync(cancellationToken);

                            if (userWeight == null)
                            {
                                continue;
                            }
                            else
                            {
                                weight = userWeight.Value;
                            }
                        }

                        if (target.TriggerOperator == TriggerOperator.GreaterThan)
                        {
                            var triggeredDays = dailyUserIngredients.Values.Where(x => x.Protein / weight > target.TriggerValue);

                            if (triggeredDays.Any())
                                bestValue = triggeredDays.Min(x => x.Protein);
                        }
                        else
                        {
                            var triggeredDays = dailyUserIngredients.Values.Where(x => x.Protein / weight < target.TriggerValue);

                            if (triggeredDays.Any())
                                bestValue = triggeredDays.Max(x => x.Protein);
                        }
                    }
                    else if (target.OptimizationArea.Type == OptimizationAreaType.Sugar)
                    {
                        if (target.TriggerOperator == TriggerOperator.GreaterThan)
                        {
                            var triggeredDays = dailyUserIngredients.Values.Where(x => x.Sugar > target.TriggerValue);

                            if (triggeredDays.Any())
                                bestValue = triggeredDays.Min(x => x.Sugar);
                        }
                        else
                        {
                            var triggeredDays = dailyUserIngredients.Values.Where(x => x.Sugar < target.TriggerValue);

                            if (triggeredDays.Any())
                                bestValue = triggeredDays.Max(x => x.Sugar);
                        }
                    }
                    else if (target.OptimizationArea.Type == OptimizationAreaType.Vegetables)
                    {
                        if (target.TriggerOperator == TriggerOperator.GreaterThan)
                        {
                            var triggeredDays = dailyUserIngredients.Values.Where(x => x.Vegetables > target.TriggerValue);

                            if (triggeredDays.Any())
                                bestValue = triggeredDays.Min(x => x.Vegetables);
                        }
                        else
                        {
                            var triggeredDays = dailyUserIngredients.Values.Where(x => x.Vegetables < target.TriggerValue);

                            if (triggeredDays.Any())
                                bestValue = triggeredDays.Max(x => x.Vegetables);
                        }
                    }

                    decimal recommendedValue = (adjustmentTarget.StepDirection == AdjustmentTargetStepDirection.Ascending)
                        ? bestValue - bestValue % adjustmentTarget.Step + adjustmentTarget.Step
                        : bestValue - bestValue % adjustmentTarget.Step;

                    targetDto.Answers = new List<TargetAnswerDto>();

                    if (target.OptimizationArea.Type == OptimizationAreaType.Protein)
                    {
                        var weightHistory = await _context.UserWeights
                            .Where(x => x.UserId == userId && x.Date > userTarget.Created.AddDays(-_statisticsPeriod) && x.Date < userTarget.Created).ToListAsync(cancellationToken);

                        decimal weight = 0;

                        if (weightHistory.Any())
                        {
                            weight = weightHistory.Average(x => x.Value);
                        }
                        else
                        {
                            weight = (await _context.UserWeights.Where(x => x.Date < userTarget.Created.AddDays(-_statisticsPeriod))
                                .OrderBy(x => x.Date).LastAsync(cancellationToken)).Value;
                        }

                        var height = (await _context.Users.SingleAsync(x => x.Id == userId, cancellationToken)).Height.Value;

                        decimal targetValue = 0;

                        if (BMI((double)height, (double)weight) < 25)
                        {
                            targetValue = weight * adjustmentTarget.TargetValue;
                        }
                        else
                        {
                            targetValue = (height - 100) * adjustmentTarget.TargetValue;
                        }

                        //TODO: use constants or enums
                        if (recommendedValue < targetValue)
                            targetDto.Answers.Add(new TargetAnswerDto { Code = "recommended", Value = string.Format(adjustmentTarget.RecommendedText, Math.Round(recommendedValue)) });

                        targetDto.Answers.Add(new TargetAnswerDto { Code = "target", Value = string.Format(adjustmentTarget.TargetText, Math.Round(targetValue)) });
                    }
                    else
                    {
                        if (recommendedValue != adjustmentTarget.TargetValue)
                            targetDto.Answers.Add(new TargetAnswerDto { Code = "recommended", Value = string.Format(adjustmentTarget.RecommendedText, Math.Round(recommendedValue)) });

                        targetDto.Answers.Add(new TargetAnswerDto { Code = "target", Value = adjustmentTarget.TargetText });
                    }

                    targetDto.Answers.Add(new TargetAnswerDto { Code = "remain", Value = adjustmentTarget.RemainText });
                }
                else
                {
                    //TODO: use constants or enums
                    targetDto.Answers = new[] {
                                new TargetAnswerDto { Code = "yes", Value ="Ja"},
                                new TargetAnswerDto { Code = "no", Value = "Nein" }
                            };
                }

                var userAnswer = await _context.UserTargets.Where(x => x.UserId == userId && x.TargetId == target.Id && x.Created > DateTime.Now.AddDays(-_statisticsPeriod)).OrderBy(x => x.Created).LastOrDefaultAsync(cancellationToken);

                targetDto.UserAnswerCode = userAnswer?.TargetAnswerCode;

                if (!result.Any(x => x.Key == target.OptimizationArea.Key))
                {
                    var optimizationAreaImage = _context.Images.SingleOrDefault(x => x.Id == target.OptimizationArea.ImageId);

                    var analysisDto = new AnalysisDto();

                    analysisDto.LineGraph = new AnalysisLineGraphDto();

                    if (target.OptimizationArea.Type == OptimizationAreaType.Protein)
                    {
                        var weightHistory = _context.UserWeights
                            .Where(x => x.UserId == userId && x.Date > userTarget.Created.AddDays(-_statisticsPeriod) && x.Date < userTarget.Created).ToList();

                        decimal weight = 0;

                        if (weightHistory.Any())
                        {
                            weight = weightHistory.Average(x => x.Value);
                        }
                        else
                        {
                            weight = _context.UserWeights.Where(x => x.Date < userTarget.Created.AddDays(-_statisticsPeriod))
                                .OrderBy(x => x.Date).Last().Value;
                        }

                        var height = (await _context.Users.SingleAsync(x => x.Id == userId, cancellationToken)).Height.Value;

                        if (BMI((double)height, (double)weight) < 25)
                        {
                            analysisDto.LineGraph.Optimal = weight * target.OptimizationArea.LineGraphOptimal;
                        }
                        else
                        {
                            analysisDto.LineGraph.Optimal = (height - 100) * target.OptimizationArea.LineGraphOptimal;
                        }

                        analysisDto.LineGraph.UpperLimit = (decimal)1.1 * analysisDto.LineGraph.Optimal.Value;
                        analysisDto.LineGraph.LowerLimit = (decimal)0.9 * analysisDto.LineGraph.Optimal.Value;

                        analysisDto.LineGraph.Data = dailyUserIngredients.Select(x => new AnalysisLineGraphDataDto
                        { Date = x.Key, Value = x.Value.Protein }).ToList();

                        var average = dailyUserIngredients.Average(x => x.Value.Protein);

                        if (average > analysisDto.LineGraph.UpperLimit)
                        {
                            analysisDto.LineGraph.Title = target.OptimizationArea.AboveOptimalLineGraphTitle;
                            analysisDto.LineGraph.Text = target.OptimizationArea.AboveOptimalLineGraphText;
                        }
                        else if (average < analysisDto.LineGraph.LowerLimit)
                        {
                            analysisDto.LineGraph.Title = target.OptimizationArea.BelowOptimalLineGraphTitle;
                            analysisDto.LineGraph.Text = target.OptimizationArea.BelowOptimalLineGraphText;
                        }
                        else
                        {
                            analysisDto.LineGraph.Title = target.OptimizationArea.OptimalLineGraphTitle;
                            analysisDto.LineGraph.Text = target.OptimizationArea.OptimalLineGraphText;
                        }

                        var totalProtein = dailyUserIngredients.Sum(x => x.Value.Protein);

                        if (totalProtein > 0)
                        {
                            analysisDto.PieChart = new AnalysisPieChartDto();
                            
                            var plantProteinPercent = (int)Math.Round(dailyUserIngredients.Sum(x => x.Value.PlantProtein) * 100 / totalProtein);

                            //TODO: use constants or enums
                            if (plantProteinPercent > 65)
                            {
                                analysisDto.PieChart.Title = target.OptimizationArea.AboveOptimalPieChartTitle;
                                analysisDto.PieChart.Text = target.OptimizationArea.AboveOptimalPieChartText;
                            }
                            else if (plantProteinPercent < 45)
                            {
                                analysisDto.PieChart.Title = target.OptimizationArea.BelowOptimalPieChartTitle;
                                analysisDto.PieChart.Text = target.OptimizationArea.BelowOptimalPieChartText;
                            }
                            else
                            {
                                analysisDto.PieChart.Title = target.OptimizationArea.OptimalPieChartTitle;
                                analysisDto.PieChart.Text = target.OptimizationArea.OptimalPieChartText;
                            }

                            //TODO: use constants or enums
                            analysisDto.PieChart.Data = new []
                            {
                                new AnalysisPieChartDataDto { Key = "animal", Value = 100 - plantProteinPercent },
                                new AnalysisPieChartDataDto { Key = "plant", Value = plantProteinPercent }
                            };
                        }
                    }
                    else if (target.OptimizationArea.Type == OptimizationAreaType.Sugar)
                    {
                        analysisDto.LineGraph.UpperLimit = target.OptimizationArea.LineGraphUpperLimit;
                        analysisDto.LineGraph.LowerLimit = target.OptimizationArea.LineGraphLowerLimit;
                        analysisDto.LineGraph.Optimal = target.OptimizationArea.LineGraphOptimal;

                        analysisDto.LineGraph.Data = dailyUserIngredients.Select(x => new AnalysisLineGraphDataDto
                            { Date = x.Key, Value = x.Value.Sugar }).ToList();

                        var average = dailyUserIngredients.Average(x => x.Value.Sugar);

                        if (average > target.OptimizationArea.LineGraphUpperLimit)
                        {
                            analysisDto.LineGraph.Title = target.OptimizationArea.AboveOptimalLineGraphTitle;
                            analysisDto.LineGraph.Text = target.OptimizationArea.AboveOptimalLineGraphText;
                        }
                        else
                        {
                            analysisDto.LineGraph.Title = target.OptimizationArea.OptimalLineGraphTitle;
                            analysisDto.LineGraph.Text = target.OptimizationArea.OptimalLineGraphText;
                        }
                    }
                    else if (target.OptimizationArea.Type == OptimizationAreaType.Vegetables)
                    {
                        analysisDto.LineGraph.UpperLimit = target.OptimizationArea.LineGraphUpperLimit;
                        analysisDto.LineGraph.LowerLimit = target.OptimizationArea.LineGraphLowerLimit;
                        analysisDto.LineGraph.Optimal = target.OptimizationArea.LineGraphOptimal;

                        analysisDto.LineGraph.Data = dailyUserIngredients.Select(x => new AnalysisLineGraphDataDto
                            { Date = x.Key, Value = x.Value.Vegetables }).ToList();

                        var average = dailyUserIngredients.Average(x => x.Value.Vegetables);

                        if (average < target.OptimizationArea.LineGraphLowerLimit)
                        {
                            analysisDto.LineGraph.Title = target.OptimizationArea.BelowOptimalLineGraphTitle;
                            analysisDto.LineGraph.Text = target.OptimizationArea.BelowOptimalLineGraphText;
                        }
                        else
                        {
                            analysisDto.LineGraph.Title = target.OptimizationArea.OptimalLineGraphTitle;
                            analysisDto.LineGraph.Text = target.OptimizationArea.OptimalLineGraphText;
                        }
                    }

                    result.Add(new OptimizationAreaDto
                    {
                        Key = target.OptimizationArea.Key,
                        Name = target.OptimizationArea.Name,
                        Text = target.OptimizationArea.Text,
                        ImageUrl = optimizationAreaImage?.Url,
                        Analysis = analysisDto,
                        Targets = new List<TargetDto>()
                    });
                }

                result.Single(x => x.Key == target.OptimizationArea.Key).Targets.Add(targetDto);
            }

            return result;
        }

        private double BMI(double height, double weight)
        {
            return (double)weight / Math.Pow((double)height / 100, 2);
        }

        private async Task<Dictionary<DateTime, MealNutritionsDto>> GetDailyUserIngredients(string userId, DateTime onDate, CancellationToken cancellationToken)
        {
            var result = new Dictionary<DateTime, MealNutritionsDto>();

            foreach (var dailyMeals in (await _context.Meals
                .Where(x => x.UserId == userId && x.Date > onDate.AddDays(-_statisticsPeriod)).ToListAsync(cancellationToken)).GroupBy(g => g.Date))
            {
                var dailyNutritions = new MealNutritionsDto
                {
                    AnimalProtein = 0,
                    PlantProtein = 0,
                    Sugar = 0,
                    Vegetables = 0
                };

                foreach (var meal in dailyMeals)
                {
                    var mealNutritions = await _foodService.GetMealNutritionsAsync(meal.Id, cancellationToken);

                    dailyNutritions.AnimalProtein += mealNutritions.AnimalProtein;
                    dailyNutritions.PlantProtein += mealNutritions.PlantProtein;
                    dailyNutritions.Sugar += mealNutritions.Sugar;
                    dailyNutritions.Vegetables += mealNutritions.Vegetables;
                }

                result[dailyMeals.Key] = dailyNutritions;
            }

            return result;
        }

        public async Task InsertAsync(string userId, InsertTargetPayload payload, CancellationToken cancellationToken)
        {
            foreach (var answer in payload.Targets)
            {
                var target = await _context.Targets.SingleOrDefaultAsync(x => x.Id == answer.TargetId, cancellationToken);

                if (target == null)
                {
                    throw new NotFoundException(nameof(Target), answer.TargetId);
                }

                var userTarget = await _context.UserTargets.Where(x => x.UserId == userId && x.TargetId == answer.TargetId && x.Created > DateTime.Now.AddDays(-_statisticsPeriod)).OrderBy(x => x.Created).LastOrDefaultAsync(cancellationToken);

                if (userTarget == null)
                {
                    await _context.UserTargets.AddAsync(new UserTarget { UserId = userId, TargetId = answer.TargetId, TargetAnswerCode = answer.UserAnswerCode }, cancellationToken);
                }
                else
                {
                    if (string.IsNullOrEmpty(answer.UserAnswerCode))
                        _context.UserTargets.Remove(userTarget);
                    else
                    {
                        userTarget.TargetAnswerCode = answer.UserAnswerCode;
                        _context.UserTargets.Update(userTarget);
                    }
                }
            }

            await _context.SaveChangesAsync(cancellationToken);
        }

        public async Task<bool> NewTriggered(string userId, CancellationToken cancellationToken)
        {
            return !(await _context.UserTargets.AnyAsync(x =>
                x.UserId == userId && x.Created > DateTime.Now.AddDays(-_statisticsPeriod), cancellationToken)) && (await GetAsync(userId, cancellationToken)).Any();
        }
        public async Task<bool> AnyAnswered(string userId, CancellationToken cancellationToken)
        {
            return await _context.UserTargets.AnyAsync(x =>
                x.UserId == userId && !string.IsNullOrEmpty(x.TargetAnswerCode), cancellationToken);
        }

        public async Task<bool> AnyActivated(string userId, CancellationToken cancellationToken)
        {
            return await _context.UserTargets.AnyAsync(x =>
                x.UserId == userId && !string.IsNullOrEmpty(x.TargetAnswerCode) && x.Created > DateTime.Now.AddDays(-_statisticsPeriod), cancellationToken);
        }
    }
}
