using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using MyFoodDoc.App.Application.Abstractions;
using MyFoodDoc.App.Application.Exceptions;
using MyFoodDoc.App.Application.Models;
using MyFoodDoc.App.Application.Payloads.Target;
using MyFoodDoc.Application.Abstractions;
using MyFoodDoc.Application.Configuration;
using MyFoodDoc.Application.Entities;
using MyFoodDoc.Application.Entities.Targets;
using MyFoodDoc.Application.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace MyFoodDoc.App.Application.Services
{
    public class TargetService : ITargetService
    {
        private readonly IApplicationContext _context;
        private readonly IFoodService _foodService;
        private readonly IDiaryService _diaryService;
        private readonly int _statisticsPeriod;
        private readonly int _statisticsMinimumDays;

        public const string VEGAN_DIET = "vegan";

        public TargetService(IApplicationContext context, IFoodService foodService, IDiaryService diaryService, IOptions<StatisticsOptions> statisticsOptions)
        {
            _context = context;
            _foodService = foodService;
            _diaryService = diaryService;
            _statisticsPeriod = statisticsOptions.Value.Period > 0 ? statisticsOptions.Value.Period : 7;
            _statisticsMinimumDays = statisticsOptions.Value.MinimumDays > 0 ? statisticsOptions.Value.MinimumDays : 3;
        }

        public async Task<ICollection<OptimizationAreaDto>> GetAsync(string userId, DateTime onDate, CancellationToken cancellationToken)
        {
            var user = await _context.Users.SingleOrDefaultAsync(x => x.Id == userId, cancellationToken);
            if (user == null)
            {
                throw new NotFoundException(nameof(User), userId);
            }

            var result = new List<OptimizationAreaDto>();

            List<UserTarget> userTargets = new List<UserTarget>();

            var userTargetsForStatisticsPeriod = await _context.UserTargets.Where(x =>
                x.UserId == userId && x.Created > onDate.AddDays(-_statisticsPeriod) && x.Created < onDate).ToListAsync(cancellationToken);

            var dailyUserIngredientsDictionary = new Dictionary<DateTime, Dictionary<DateTime, MealNutritionsDto>>();

            if (userTargetsForStatisticsPeriod.Any())
            {
                userTargets = userTargetsForStatisticsPeriod
                    .GroupBy(g => g.TargetId).Select(x => x.OrderBy(y => y.Created).Last()).ToList();
            }
            else
            {
                if (!await _diaryService.IsDiaryFull(userId, onDate, cancellationToken))
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

                var dailyUserIngredients = await GetDailyUserIngredients(userId, onDate, cancellationToken);

                var dateKey = new DateTime(onDate.Year, onDate.Month, onDate.Day);

                dailyUserIngredientsDictionary[dateKey] = dailyUserIngredients;

                foreach (var dailyMeals in _context.Meals
                    .Where(x => x.UserId == userId && x.Date > dateKey.AddDays(-_statisticsPeriod) && x.Date <= dateKey).ToList().GroupBy(g => g.Date))
                {
                    var dailyNutritions = new MealNutritionsDto
                    {
                        AnimalProtein = 0,
                        PlantProtein = 0,
                        Sugar = 0,
                        Vegetables = 0,
                        Meals = dailyMeals.Count()
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
                            .Where(x => x.UserId == userId && x.Date > dateKey.AddDays(-_statisticsPeriod) && x.Date <= dateKey).ToListAsync(cancellationToken);

                        decimal weight = 0;

                        if (weightHistory.Any())
                        {
                            weight = weightHistory.Average(x => x.Value);
                        }
                        else
                        {
                            var userWeight = _context.UserWeights.Where(x => x.UserId == userId && x.Date <= dateKey.AddDays(-_statisticsPeriod))
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

                        var correctedWeight = _diaryService.GetCorrectedWeight(user.Height.Value, weight);
                        if (correctedWeight <= 0)
                        {
                            continue;
                        }

                        if (target.TriggerOperator == TriggerOperator.GreaterThan)
                        {
                            var triggeredDays = dailyUserIngredients.Values.Where(x => x.Protein / correctedWeight > target.TriggerValue);

                            triggeredDaysCount = triggeredDays.Count();
                        }
                        else
                        {
                            var triggeredDays = dailyUserIngredients.Values.Where(x => x.Protein / correctedWeight < target.TriggerValue);

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
                    else if (target.OptimizationArea.Type == OptimizationAreaType.Snacking)
                    {
                        if (target.TriggerOperator == TriggerOperator.GreaterThan)
                        {
                            var triggeredDays = dailyUserIngredients.Values.Where(x => x.Meals > target.TriggerValue);

                            triggeredDaysCount = triggeredDays.Count();
                        }
                        else
                        {
                            var triggeredDays = dailyUserIngredients.Values.Where(x => x.Meals < target.TriggerValue);

                            triggeredDaysCount = triggeredDays.Count();
                        }
                    }

                    var frequency = (decimal)triggeredDaysCount * 100 / _statisticsPeriod;

                    if (frequency > target.Threshold)
                    {
                        var userTarget = new UserTarget { UserId = userId, TargetId = target.Id, Created = onDate };

                        userTargets.Add(userTarget);
                    }
                }

                await _context.SaveChangesAsync(cancellationToken);
            }

            bool isVegan = await _diaryService.CheckDiet(user.Id, VEGAN_DIET, cancellationToken);
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

                var dateKey = new DateTime(userTarget.Created.Year, userTarget.Created.Month, userTarget.Created.Day);

                if (!dailyUserIngredientsDictionary.ContainsKey(dateKey))
                    dailyUserIngredientsDictionary[dateKey] = await GetDailyUserIngredients(userId, userTarget.Created, cancellationToken);

                var dailyUserIngredients = dailyUserIngredientsDictionary[dateKey];

                if (target.Type == TargetType.Adjustment)
                {
                    var adjustmentTarget = await _context.AdjustmentTargets.SingleAsync(x => x.TargetId == target.Id, cancellationToken);

                    decimal bestValue = 0;

                    if (target.OptimizationArea.Type == OptimizationAreaType.Protein)
                    {
                        var userTargetDate = userTarget.Created.Date;

                        var weightHistory = await _context.UserWeights
                            .Where(x => x.UserId == userId && x.Date > userTargetDate.AddDays(-_statisticsPeriod) && x.Date <= userTargetDate).ToListAsync(cancellationToken);

                        decimal weight = 0;

                        if (weightHistory.Any())
                        {
                            weight = weightHistory.Average(x => x.Value);
                        }
                        else
                        {
                            var userWeight = await _context.UserWeights.Where(x => x.UserId == userId && x.Date <= userTargetDate.AddDays(-_statisticsPeriod))
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

                        var correctedWeight = _diaryService.GetCorrectedWeight(user.Height.Value, weight);

                        if (target.TriggerOperator == TriggerOperator.GreaterThan)
                        {
                            var triggeredDays = dailyUserIngredients.Values.Where(x => x.Protein / correctedWeight > target.TriggerValue);

                            if (triggeredDays.Any())
                                bestValue = triggeredDays.Min(x => x.Protein);
                        }
                        else
                        {
                            var triggeredDays = dailyUserIngredients.Values.Where(x => x.Protein / correctedWeight < target.TriggerValue);

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
                    else if (target.OptimizationArea.Type == OptimizationAreaType.Snacking)
                    {
                        if (target.TriggerOperator == TriggerOperator.GreaterThan)
                        {
                            var triggeredDays = dailyUserIngredients.Values.Where(x => x.Meals > target.TriggerValue);

                            if (triggeredDays.Any())
                                bestValue = triggeredDays.Min(x => x.Meals);
                        }
                        else
                        {
                            var triggeredDays = dailyUserIngredients.Values.Where(x => x.Meals < target.TriggerValue);

                            if (triggeredDays.Any())
                                bestValue = triggeredDays.Max(x => x.Meals);
                        }
                    }

                    decimal recommendedValue = (adjustmentTarget.StepDirection == AdjustmentTargetStepDirection.Ascending)
                        ? bestValue - bestValue % adjustmentTarget.Step + adjustmentTarget.Step
                        : bestValue - bestValue % adjustmentTarget.Step;

                    targetDto.Answers = new List<TargetAnswerDto>();

                    if (target.OptimizationArea.Type == OptimizationAreaType.Protein)
                    {
                        var userTargetDate = userTarget.Created.Date;

                        var weightHistory = await _context.UserWeights
                            .Where(x => x.UserId == userId && x.Date > userTargetDate.AddDays(-_statisticsPeriod) && x.Date <= userTargetDate).ToListAsync(cancellationToken);

                        decimal weight = 0;

                        if (weightHistory.Any())
                        {
                            weight = weightHistory.Average(x => x.Value);
                        }
                        else
                        {
                            weight = (await _context.UserWeights.Where(x => x.UserId == userId && x.Date <= userTargetDate.AddDays(-_statisticsPeriod))
                                .OrderBy(x => x.Date).LastAsync(cancellationToken)).Value;
                        }

                        decimal targetValue = _diaryService.GetCorrectedWeight(user.Height.Value, weight) * adjustmentTarget.TargetValue;

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

                var userAnswer = await _context.UserTargets.Where(x => x.UserId == userId && x.TargetId == target.Id && x.Created > onDate.AddDays(-_statisticsPeriod) && x.Created < onDate).OrderBy(x => x.Created).LastOrDefaultAsync(cancellationToken);

                targetDto.UserAnswerCode = userAnswer?.TargetAnswerCode;

                if (!result.Any(x => x.Key == target.OptimizationArea.Key))
                {
                    var optimizationAreaImage = _context.Images.SingleOrDefault(x => x.Id == target.OptimizationArea.ImageId);

                    var analysisDto = new AnalysisDto();

                    analysisDto.LineGraph = new AnalysisLineGraphDto();

                    if (target.OptimizationArea.Type == OptimizationAreaType.Protein)
                    {
                        var userTargetDate = userTarget.Created.Date;

                        var weightHistory = _context.UserWeights
                            .Where(x => x.UserId == userId && x.Date > userTargetDate.AddDays(-_statisticsPeriod) && x.Date <= userTargetDate).ToList();

                        decimal weight = 0;

                        if (weightHistory.Any())
                        {
                            weight = weightHistory.Average(x => x.Value);
                        }
                        else
                        {
                            weight = _context.UserWeights.Where(x => x.UserId == userId && x.Date <= userTargetDate.AddDays(-_statisticsPeriod))
                                .OrderBy(x => x.Date).Last().Value;
                        }

                        var correctedWeight = _diaryService.GetCorrectedWeight(user.Height.Value, weight);

                        analysisDto.LineGraph.UpperLimit = correctedWeight * target.OptimizationArea.LineGraphUpperLimit.Value;
                        analysisDto.LineGraph.LowerLimit = correctedWeight * target.OptimizationArea.LineGraphLowerLimit.Value;
                        analysisDto.LineGraph.Optimal = correctedWeight * target.OptimizationArea.LineGraphOptimal.Value;

                        analysisDto.LineGraph.Data = dailyUserIngredients.Select(x => new AnalysisLineGraphDataDto
                        { Date = x.Key, Value = x.Value.Protein }).ToList();

                        var average = dailyUserIngredients.Average(x => x.Value.Protein);

                        if (average >= analysisDto.LineGraph.UpperLimit)
                        {
                            analysisDto.LineGraph.Title = target.OptimizationArea.AboveOptimalLineGraphTitle;
                            analysisDto.LineGraph.Text = target.OptimizationArea.AboveOptimalLineGraphText;
                        }
                        else if (average <= analysisDto.LineGraph.LowerLimit)
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

                        // MFD-1496 : if the user has vegan Diet the array with the optimization MUST NOT have the piechart info 
                        if (totalProtein > 0 && !isVegan)
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
                            analysisDto.PieChart.Data = new[]
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

                        if (average >= target.OptimizationArea.LineGraphUpperLimit)
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

                        if (average <= target.OptimizationArea.LineGraphLowerLimit)
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
                    else if (target.OptimizationArea.Type == OptimizationAreaType.Snacking)
                    {
                        analysisDto.LineGraph.UpperLimit = target.OptimizationArea.LineGraphUpperLimit;
                        analysisDto.LineGraph.LowerLimit = target.OptimizationArea.LineGraphLowerLimit;
                        analysisDto.LineGraph.Optimal = target.OptimizationArea.LineGraphOptimal;

                        analysisDto.LineGraph.Data = dailyUserIngredients.Select(x => new AnalysisLineGraphDataDto
                        { Date = x.Key, Value = x.Value.Meals }).ToList();

                        var average = (decimal)dailyUserIngredients.Average(x => x.Value.Meals);

                        if (average <= target.OptimizationArea.LineGraphLowerLimit)
                        {
                            analysisDto.LineGraph.Title = target.OptimizationArea.BelowOptimalLineGraphTitle;
                            analysisDto.LineGraph.Text = target.OptimizationArea.BelowOptimalLineGraphText;
                        }
                        else if (average >= target.OptimizationArea.LineGraphUpperLimit)
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

        private async Task<Dictionary<DateTime, MealNutritionsDto>> GetDailyUserIngredients(string userId, DateTime onDateTime, CancellationToken cancellationToken)
        {
            var result = new Dictionary<DateTime, MealNutritionsDto>();

            var onDate = new DateTime(onDateTime.Year, onDateTime.Month, onDateTime.Day);

            foreach (var dailyMeals in (await _context.Meals
                .Where(x => x.UserId == userId && x.Date > onDate.AddDays(-_statisticsPeriod) && x.Date <= onDate).ToListAsync(cancellationToken)).GroupBy(g => g.Date))
            {
                var dailyNutritions = new MealNutritionsDto
                {
                    AnimalProtein = 0,
                    PlantProtein = 0,
                    Sugar = 0,
                    Vegetables = 0,
                    Meals = dailyMeals.Count()
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
                x.UserId == userId && x.Created > DateTime.Now.AddDays(-_statisticsPeriod), cancellationToken)) && (await GetAsync(userId, DateTime.Now, cancellationToken)).Any();
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

        public async Task<int> GetDaysTillFirstEvaluationAsync(string userId, CancellationToken cancellationToken)
        {
            var user = await _context.Users
                .Where(x => x.Id == userId)
                .SingleOrDefaultAsync(cancellationToken);

            if (user == null)
            {
                throw new NotFoundException(nameof(User), userId);
            }

            if (await AnyAnswered(userId, cancellationToken))
                return 0;

            var dateToCheck = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);

            var userCreatedDate = new DateTime(user.Created.Year, user.Created.Month, user.Created.Day);

            var daysSinceCreation = (dateToCheck - userCreatedDate).Days;

            var diaryRecords = await _context.Meals.Where(x => x.UserId == user.Id && x.Date > dateToCheck.AddDays(-_statisticsPeriod) && x.Date <= dateToCheck).ToListAsync(cancellationToken);

            var daysRecorded = diaryRecords.Select(x => x.Date)
                .Distinct()
                .Count();

            if (daysSinceCreation <= _statisticsPeriod - _statisticsMinimumDays)
                return _statisticsPeriod - daysSinceCreation;
            else if (daysSinceCreation > _statisticsPeriod - _statisticsMinimumDays && daysSinceCreation < _statisticsPeriod)
            {
                if (daysRecorded + _statisticsPeriod - daysSinceCreation >= _statisticsMinimumDays)
                    return _statisticsPeriod - daysSinceCreation;
                else
                    return _statisticsMinimumDays - daysRecorded;
            }
            else
            {
                if (daysRecorded >= _statisticsMinimumDays)
                    return 0;
                else
                {
                    for (int i = 1; i < _statisticsMinimumDays; i++)
                    {
                        daysRecorded = diaryRecords.Where(x =>
                                x.Date >= dateToCheck.AddDays(-_statisticsPeriod + i))
                            .Select(x => x.Date)
                            .Distinct()
                            .Count();

                        if (daysRecorded + i == _statisticsMinimumDays)
                            return i;
                    }

                    return _statisticsMinimumDays;
                }
            }
        }
    }
}
