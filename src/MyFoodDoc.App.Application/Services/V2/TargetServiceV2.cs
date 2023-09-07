using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using MyFoodDoc.App.Application.Abstractions.V2;
using MyFoodDoc.App.Application.Exceptions;
using MyFoodDoc.App.Application.Models;
using MyFoodDoc.App.Application.Models.StatisticsDto;
using MyFoodDoc.App.Application.Payloads.Target;
using MyFoodDoc.Application.Abstractions;
using MyFoodDoc.Application.Configuration;
using MyFoodDoc.Application.Entities;
using MyFoodDoc.Application.Entities.Targets;
using MyFoodDoc.Application.Enums;
using Spire.Pdf.Exporting.XPS.Schema;

namespace MyFoodDoc.App.Application.Services.V2
{
    public class TargetServiceV2 : ITargetServiceV2
    {
        private readonly IApplicationContext _context;
        private readonly IFoodServiceV2 _foodService;
        private readonly IDiaryServiceV2 _diaryService;
        private readonly int _statisticsPeriod;
        private readonly int _statisticsMinimumDays;

        public const string VEGAN_DIET = "vegan";

        public TargetServiceV2(IApplicationContext context,
            IFoodServiceV2 foodService,
            IDiaryServiceV2 diaryService,
            IOptions<StatisticsOptions> statisticsOptions)
        {
            _context = context;
            _foodService = foodService;
            _diaryService = diaryService;
            _statisticsPeriod = statisticsOptions.Value.Period > 0 ? statisticsOptions.Value.Period : 7;
            _statisticsMinimumDays = statisticsOptions.Value.MinimumDays > 0 ? statisticsOptions.Value.MinimumDays : 3;
        }

        private OptimizationAreaDto Calculate(
            StatisticsUserDto user,
            UserTargetDto userTarget,
            bool isVegan,
            Dictionary<DateTime, Dictionary<DateTime, MealNutritionsDto>> dailyUserIngredientsDictionary)
        {
            var target = user.UserTargets?
                .FirstOrDefault(x => x.TargetId == userTarget.TargetId)?.Target;
            if (target is null)
            {
                return null;
            }

            var targetDto = new TargetDto
            {
                Id = target.Id,
                Type = target.Type.ToString(),
                Title = target.Title,
                Text = target.Text,
                ImageUrl = target.ImageUrl
            };

            var dateKey = new DateTime(userTarget.Created.Year, userTarget.Created.Month, userTarget.Created.Day);

            if (!dailyUserIngredientsDictionary.ContainsKey(dateKey))
                dailyUserIngredientsDictionary[dateKey] =
                    GetDailyUserIngredients(user.Meals, user.FavouriteIngredientDtos, userTarget.Created);

            var dailyUserIngredients = dailyUserIngredientsDictionary[dateKey];

            if (target.Type == TargetType.Adjustment)
            {
                AdjustmentTargetDto adjustmentTarget = target.AdjustmentTargetDto;

                decimal bestValue = GetBestValueOfTriggeredDays(user, target, userTarget.Created, dailyUserIngredients);


                decimal recommendedValue = (adjustmentTarget.StepDirection == AdjustmentTargetStepDirection.Ascending)
                    ? bestValue - bestValue % adjustmentTarget.Step + adjustmentTarget.Step
                    : bestValue - bestValue % adjustmentTarget.Step;

                targetDto.Answers = GetUserTargetAnswers(user, target, userTarget.Created.Date, adjustmentTarget,
                    recommendedValue);
            }
            else
            {
                targetDto.Answers = new[]
                {
                    new TargetAnswerDto {Code = "yes", Value = "Ja"},
                    new TargetAnswerDto {Code = "no", Value = "Nein"}
                };
            }

            targetDto.UserAnswerCode = userTarget?.TargetAnswerCode;


            var optimizationAreaImage =
                _context.Images.SingleOrDefault(x => x.Id == target.OptimizationArea.ImageId);

            var analysisDto = new AnalysisDto();

            analysisDto.LineGraph = new AnalysisLineGraphDto();

            if (target.OptimizationArea.Type == OptimizationAreaType.Protein)
            {
                var userTargetDate = userTarget.Created.Date;

                var weightHistory = user.Weights
                    .Where(x => x.Date > userTargetDate.AddDays(-_statisticsPeriod) &&
                                x.Date <= userTargetDate).ToList();

                decimal weight = 0;

                if (weightHistory.Any())
                {
                    weight = weightHistory.Average(x => x.Value);
                }
                else
                {
                    weight = user.Weights.Where(x => x.Date <= userTargetDate.AddDays(-_statisticsPeriod))
                        .OrderBy(x => x.Date).Last().Value;
                }

                var correctedWeight = _diaryService.GetCorrectedWeight(user.Height.Value, weight);

                analysisDto.LineGraph.UpperLimit =
                    correctedWeight * target.OptimizationArea.LineGraphUpperLimit.Value;
                analysisDto.LineGraph.LowerLimit =
                    correctedWeight * target.OptimizationArea.LineGraphLowerLimit.Value;
                analysisDto.LineGraph.Optimal = correctedWeight * target.OptimizationArea.LineGraphOptimal.Value;

                analysisDto.LineGraph.Data = dailyUserIngredients.Select(x => new AnalysisLineGraphDataDto
                    {Date = x.Key, Value = x.Value.Protein}).ToList();

                var average = dailyUserIngredients.Count > 0
                    ? dailyUserIngredients.Average(x => x.Value.Protein)
                    : 0;

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

                    var plantProteinPercent =
                        (int) Math.Round(dailyUserIngredients.Sum(x => x.Value.PlantProtein) * 100 / totalProtein);

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

                    analysisDto.PieChart.Data = new[]
                    {
                        new AnalysisPieChartDataDto {Key = "animal", Value = 100 - plantProteinPercent},
                        new AnalysisPieChartDataDto {Key = "plant", Value = plantProteinPercent}
                    };
                }
            }
            else if (target.OptimizationArea.Type == OptimizationAreaType.Sugar)
            {
                analysisDto.LineGraph.UpperLimit = target.OptimizationArea.LineGraphUpperLimit;
                analysisDto.LineGraph.LowerLimit = target.OptimizationArea.LineGraphLowerLimit;
                analysisDto.LineGraph.Optimal = target.OptimizationArea.LineGraphOptimal;

                analysisDto.LineGraph.Data = dailyUserIngredients.Select(x => new AnalysisLineGraphDataDto
                    {Date = x.Key, Value = x.Value.Sugar}).ToList();

                var average = dailyUserIngredients.Count > 0 ? dailyUserIngredients.Average(x => x.Value.Sugar) : 0;

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

                analysisDto.LineGraph.Data = dailyUserIngredients.Select(x =>
                    new AnalysisLineGraphDataDto
                        {Date = x.Key, Value = x.Value.Vegetables}).ToList();

                var average = dailyUserIngredients.Count > 0
                    ? dailyUserIngredients.Average(x => x.Value.Vegetables)
                    : 0;

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
                    {Date = x.Key, Value = x.Value.Meals}).ToList();

                var average = dailyUserIngredients.Count > 0
                    ? (decimal) dailyUserIngredients.Average(x => x.Value.Meals)
                    : 0;

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

            return new OptimizationAreaDto
            {
                Key = target.OptimizationArea.Key,
                Name = target.OptimizationArea.Name,
                Text = target.OptimizationArea.Text,
                ImageUrl = optimizationAreaImage?.Url,
                Analysis = analysisDto,
                Targets = new List<TargetDto>()
            };
        }

        private ICollection<TargetAnswerDto> GetUserTargetAnswers(
            StatisticsUserDto user,
            FullUserTargetDto target,
            DateTime userTargetDate,
            AdjustmentTargetDto adjustmentTarget,
            decimal recommendedValue)
        {
            var answers = new List<TargetAnswerDto>();

            if (target.OptimizationArea.Type == OptimizationAreaType.Protein)
            {
                var weightHistory = user.Weights
                    .Where(x => x.Date > userTargetDate.AddDays(-_statisticsPeriod) &&
                                x.Date <= userTargetDate);

                decimal weight = 0;

                if (weightHistory.Any())
                {
                    weight = weightHistory.Average(x => x.Value);
                }
                else
                {
                    weight = user.Weights.Where(x => x.Date <= userTargetDate.AddDays(-_statisticsPeriod))
                        .OrderBy(x => x.Date).Last().Value;
                }

                decimal targetValue = _diaryService.GetCorrectedWeight(user.Height.Value, weight) *
                                      adjustmentTarget.TargetValue;

                if (recommendedValue < targetValue)
                    answers.Add(new TargetAnswerDto
                    {
                        Code = "recommended",
                        Value = string.Format(adjustmentTarget.RecommendedText, Math.Round(recommendedValue))
                    });

                answers.Add(new TargetAnswerDto
                    {Code = "target", Value = string.Format(adjustmentTarget.TargetText, Math.Round(targetValue))});
            }
            else
            {
                if (recommendedValue != adjustmentTarget.TargetValue)
                    answers.Add(new TargetAnswerDto
                    {
                        Code = "recommended",
                        Value = string.Format(adjustmentTarget.RecommendedText, Math.Round(recommendedValue))
                    });

                answers.Add(new TargetAnswerDto {Code = "target", Value = adjustmentTarget.TargetText});
            }

            answers.Add(new TargetAnswerDto {Code = "remain", Value = adjustmentTarget.RemainText});
            return answers;
        }

        private decimal GetBestValueOfTriggeredDays(
            StatisticsUserDto user,
            FullUserTargetDto target,
            DateTime userTargetDate,
            Dictionary<DateTime, MealNutritionsDto> dailyUserIngredients)
        {
            switch (target.OptimizationArea.Type)
            {
                case OptimizationAreaType.Protein:
                {
                    var weightHistory = user.Weights
                        .Where(x => x.Date > userTargetDate.AddDays(-_statisticsPeriod) && x.Date <= userTargetDate)
                        .ToArray();

                    var weight = CalculateWeightFromWeightHistory(weightHistory, user.Weights, userTargetDate);


                    var correctedWeight = _diaryService.GetCorrectedWeight(user.Height.Value, weight);

                    if (target.TriggerOperator == TriggerOperator.GreaterThan)
                    {
                        var triggeredDays =
                            dailyUserIngredients.Values.Where(x => x.Protein / correctedWeight > target.TriggerValue);

                        if (triggeredDays.Any())
                            return triggeredDays.Min(x => x.Protein);
                    }
                    else if (target.TriggerOperator == TriggerOperator.LessThan)
                    {
                        var triggeredDays =
                            dailyUserIngredients.Values.Where(x => x.Protein / correctedWeight < target.TriggerValue);

                        if (triggeredDays.Any())
                            return triggeredDays.Max(x => x.Protein);
                    }

                    break;
                }
                case OptimizationAreaType.Sugar when target.TriggerOperator == TriggerOperator.GreaterThan:
                {
                    var triggeredDays = dailyUserIngredients.Values.Where(x => x.Sugar > target.TriggerValue);

                    if (triggeredDays.Any())
                        return triggeredDays.Min(x => x.Sugar);
                    break;
                }
                case OptimizationAreaType.Sugar:
                {
                    if (target.TriggerOperator == TriggerOperator.LessThan)
                    {
                        var triggeredDays = dailyUserIngredients.Values.Where(x => x.Sugar < target.TriggerValue);

                        if (triggeredDays.Any())
                            return triggeredDays.Max(x => x.Sugar);
                    }

                    break;
                }
                case OptimizationAreaType.Vegetables when target.TriggerOperator == TriggerOperator.GreaterThan:
                {
                    var triggeredDays = dailyUserIngredients.Values.Where(x => x.Vegetables > target.TriggerValue);

                    if (triggeredDays.Any())
                        return triggeredDays.Min(x => x.Vegetables);
                    break;
                }
                case OptimizationAreaType.Vegetables:
                {
                    if (target.TriggerOperator == TriggerOperator.LessThan)
                    {
                        var triggeredDays = dailyUserIngredients.Values.Where(x => x.Vegetables < target.TriggerValue);

                        if (triggeredDays.Any())
                            return triggeredDays.Max(x => x.Vegetables);
                    }

                    break;
                }
                case OptimizationAreaType.Snacking when target.TriggerOperator == TriggerOperator.GreaterThan:
                {
                    var triggeredDays = dailyUserIngredients.Values.Where(x => x.Meals > target.TriggerValue);

                    if (triggeredDays.Any())
                        return triggeredDays.Min(x => x.Meals);
                    break;
                }
                case OptimizationAreaType.Snacking:
                {
                    if (target.TriggerOperator == TriggerOperator.LessThan)
                    {
                        var triggeredDays = dailyUserIngredients.Values.Where(x => x.Meals < target.TriggerValue);

                        if (triggeredDays.Any())
                            return triggeredDays.Max(x => x.Meals);
                    }

                    break;
                }
                default:
                    return default;
            }

            return default;
        }

        private decimal CalculateWeightFromWeightHistory(UserWeightDto[] weightHistory,
            IEnumerable<UserWeightDto> userWeights, DateTime userTargetDate)
        {
            if (weightHistory.Any())
            {
                return weightHistory.Average(x => x.Value);
            }
            var userWeight = userWeights
                .Where(x => x.Date <= userTargetDate.AddDays(-_statisticsPeriod))
                .MaxBy(x => x.Date);

            return userWeight?.Value ?? default;

        }

        public IEnumerable<OptimizationAreaDto> Get(StatisticsUserDto user, string userId, DateTime? onDate)
        {
            var date = onDate ?? DateTime.Today.AddMinutes(-1);

            if (user == null)
            {
                throw new NotFoundException(nameof(User), userId);
            }


            if (!_diaryService.IsDiaryFull(user.Meals, user.Created, date))
            {
                return Array.Empty<OptimizationAreaDto>();
            }

            var targets = GetTargetIds(user).ToList();

            if (!targets.Any())
            {
                return Array.Empty<OptimizationAreaDto>();
            }
            var dailyUserNutritions = GetDailyUserIngredients(user.Meals, user.FavouriteIngredientDtos, date);
            var dateKey = date.Date;

            var userNutritionsForSpecificDate = GetUserNutritions(
                user.Meals,
                user.FavouriteIngredientDtos,
                date);

            var dailyUserIngredients = new Dictionary<DateTime, MealNutritionsDto>();

            foreach (var item in dailyUserNutritions) dailyUserIngredients[item.Key] = item.Value;
            foreach (var item in userNutritionsForSpecificDate)
            {
                dailyUserIngredients.TryAdd(item.Key, item.Value);
            }

            var dailyUserIngredientsDictionary = new Dictionary<DateTime, Dictionary<DateTime, MealNutritionsDto>>
            {
                { dateKey, dailyUserIngredients }
            };

            if (!dailyUserIngredients.Any())
            {
                return Array.Empty<OptimizationAreaDto>();
            }

            return GetOptimizationArea(user, targets, date, dailyUserIngredients,
                dailyUserIngredientsDictionary);
        }

        private ICollection<OptimizationAreaDto> GetOptimizationArea(
            StatisticsUserDto user,
            ICollection<TargetDetailsDto> targets,
            DateTime date,
            Dictionary<DateTime, MealNutritionsDto> dailyUserIngredients,
            Dictionary<DateTime, Dictionary<DateTime, MealNutritionsDto>> dailyUserIngredientsDictionary)
        {
            var userTargets = UserTargets(
                targets,
                date,
                user,
                dailyUserIngredients);

            var isVegan = user.Diets.Any(x => x.Key == VEGAN_DIET);
            var result = new List<OptimizationAreaDto>();
            foreach (var userTarget in userTargets)
            {
                var userAnswer = 
                user.UserTargets
                    .Where(x => x.TargetId == userTarget.TargetId &&
                                x.Created > date.AddDays(-_statisticsPeriod) && x.Created < date)
                    .MaxBy(x => x.Created);

                var r = Calculate(user, userAnswer, isVegan,
                    dailyUserIngredientsDictionary);
                result.Add(r);
            }

            return result;
        }

        private IEnumerable<UserTargetDto> UserTargets(
            IEnumerable<TargetDetailsDto> targetList,
            DateTime dateKey,
            StatisticsUserDto user,
            Dictionary<DateTime, MealNutritionsDto> dailyUserIngredients)
        {
            foreach (var target in targetList)
            {
                if (TryGetTriggeredDaysCount(target, dateKey, user, dailyUserIngredients,
                        out var triggeredDaysCount))
                {
                    continue;
                }

                var frequency = (decimal) triggeredDaysCount * 100 / _statisticsPeriod;

                if (target.TriggerOperator != TriggerOperator.Always && frequency <= target.Threshold) continue;

                var userTarget = user.UserTargets.FirstOrDefault(x => x.TargetId == target.Id);

                yield return userTarget;
            }
        }

        private bool TryGetTriggeredDaysCount(TargetDetailsDto target, DateTime dateKey, StatisticsUserDto user,
            Dictionary<DateTime, MealNutritionsDto> dailyUserIngredients, out int count)
        {
            switch (target.OptimizationArea.Type)
            {
                case OptimizationAreaType.Protein 
                    when TryCountTriggeredDaysProtein(
                        dateKey, 
                        user, 
                        dailyUserIngredients,
                        target,
                        out var triggeredDaysCount):
                {
                    count = triggeredDaysCount;
                    return true;
                }
                case OptimizationAreaType.Sugar when target.TriggerOperator == TriggerOperator.GreaterThan:
                {
                    var triggeredDays =
                        dailyUserIngredients.Values.Where(x => x.Sugar > target.TriggerValue);

                    count = triggeredDays.Count();
                    return true;
                }
                case OptimizationAreaType.Sugar when target.TriggerOperator == TriggerOperator.LessThan:
                {
                    var triggeredDays =
                        dailyUserIngredients.Values.Where(x => x.Sugar < target.TriggerValue);

                    count = triggeredDays.Count();
                    return true;
                }
                case OptimizationAreaType.Vegetables when target.TriggerOperator == TriggerOperator.GreaterThan:
                {
                    var triggeredDays = dailyUserIngredients.Values.Where(x => x.Vegetables > target.TriggerValue);

                    count = triggeredDays.Count();
                    return true;
                }
                case OptimizationAreaType.Vegetables when target.TriggerOperator == TriggerOperator.LessThan:
                {
                    var triggeredDays = dailyUserIngredients.Values.Where(x => x.Vegetables < target.TriggerValue);

                    count = triggeredDays.Count();
                    return true;
                }
                case OptimizationAreaType.Snacking when target.TriggerOperator == TriggerOperator.GreaterThan:
                {
                    var triggeredDays = dailyUserIngredients.Values.Where(x => x.Meals > target.TriggerValue);

                    count = triggeredDays.Count();
                    return true;
                }
                case OptimizationAreaType.Snacking when target.TriggerOperator == TriggerOperator.LessThan:
                {
                    var triggeredDays = dailyUserIngredients.Values.Where(x => x.Meals < target.TriggerValue);

                    count = triggeredDays.Count();
                    return true;
                }
                default:
                    count = 0;
                    return false;
            }
        }

        private bool TryCountTriggeredDaysProtein(
            DateTime dateKey,
            StatisticsUserDto user,
            Dictionary<DateTime, MealNutritionsDto> dailyUserIngredients,
            TargetDetailsDto target,
            out int triggeredDaysCount)
        {
            triggeredDaysCount = 0;
            var weightHistory = user.Weights
                .Where(x => x.Date > dateKey.AddDays(-_statisticsPeriod) &&
                            x.Date <= dateKey).ToArray();

            decimal weight = 0;

            if (!weightHistory.Any())
            {
                var userWeight = user.Weights
                    .Where(x => x.Date <= dateKey.AddDays(-_statisticsPeriod))
                    .MaxBy(x => x.Date);

                if (userWeight == null)
                {
                    return true;
                }

                weight = userWeight.Value;
            }
            else
            {
                weight = weightHistory.Average(x => x.Value);
            }

            var correctedWeight = _diaryService.GetCorrectedWeight(user.Height.Value, weight);
            if (correctedWeight <= 0)
            {
                return true;
            }

            if (target.TriggerOperator == TriggerOperator.GreaterThan)
            {
                var triggeredDays =
                    dailyUserIngredients.Values.Where(x => x.Protein / correctedWeight > target.TriggerValue);

                triggeredDaysCount = triggeredDays.Count();
            }
            else if (target.TriggerOperator == TriggerOperator.LessThan)
            {
                var triggeredDays =
                    dailyUserIngredients.Values.Where(x => x.Protein / correctedWeight < target.TriggerValue);

                triggeredDaysCount = triggeredDays.Count();
            }

            return false;
        }

        private Dictionary<DateTime, MealNutritionsDto> GetUserNutritions(IEnumerable<MealDto> meals,
            UserFavouriteDto userFavouriteDtos,
            DateTime dateKey)
        {
            var dailyUserNutritions = new Dictionary<DateTime, MealNutritionsDto>();
            foreach (var dailyMeals in meals.Where(x =>
                             x.Date > dateKey.AddDays(-_statisticsPeriod) &&
                             x.Date <= dateKey)
                         .GroupBy(g => g.Date))
            {
                var dailyNutrition = new MealNutritionsDto
                {
                    AnimalProtein = 0,
                    PlantProtein = 0,
                    Sugar = 0,
                    Vegetables = 0,
                    Meals = dailyMeals.Count()
                };

                foreach (var meal in dailyMeals)
                {
                    var mealNutritions = _foodService.GetMealNutritionsAsync(meal, userFavouriteDtos);

                    dailyNutrition.AnimalProtein += mealNutritions.AnimalProtein;
                    dailyNutrition.PlantProtein += mealNutritions.PlantProtein;
                    dailyNutrition.Sugar += mealNutritions.Sugar;
                    dailyNutrition.Vegetables += mealNutritions.Vegetables;
                }

                dailyUserNutritions[dailyMeals.Key] = dailyNutrition;
            }

            return dailyUserNutritions;
        }

        private IEnumerable<TargetDetailsDto> GetTargetIds(StatisticsUserDto user)
        {

            var dietTargets = user.Diets.SelectMany(x => x.Targets);

            var indicationTargets = user.IndicationTargets; 

            var motivationTargets = user.MotivationTargets; 

            var targetIds = dietTargets
                .Union(indicationTargets)
                .Union(motivationTargets).Distinct().ToList();
            return targetIds;
        }

        public ICollection<OptimizationAreaDto> GetLastAsync(
            StatisticsUserDto user)
        {
            var dailyUserIngredientsDictionary = new Dictionary<DateTime, Dictionary<DateTime, MealNutritionsDto>>();

            var lastDate = user.UserTargets
                .Select(x => x.Created)
                .OrderByDescending(x => x)
                .Take(1)
                .FirstOrDefault();

            if (lastDate == DateTime.MinValue)
            {
                return Array.Empty<OptimizationAreaDto>();
            }

            var targets = user.UserTargets
                .Where(x => x.Created >= lastDate.Date)
                .ToList();

            var isVegan = user.Diets.Any(x => x.Key == VEGAN_DIET);

            return targets.Select(x => Calculate(user, x, isVegan, dailyUserIngredientsDictionary)).ToList();
        }


        private Dictionary<DateTime, MealNutritionsDto> GetDailyUserIngredients(IEnumerable<MealDto> meals, UserFavouriteDto userFavouriteDtos, DateTime onDate)
        {
            var result = new Dictionary<DateTime, MealNutritionsDto>();

            var dailyMeals = meals
                .Where(x => x.Date > onDate.AddDays(-_statisticsPeriod) && x.Date <= onDate.Date)
                .GroupBy(g => g.Date);
            
            foreach (var dailyMeal in dailyMeals)
            {
                var dailyNutritions = new MealNutritionsDto
                {
                    AnimalProtein = 0,
                    PlantProtein = 0,
                    Sugar = 0,
                    Vegetables = 0,
                    Meals = dailyMeal.Count()
                };

                foreach (var meal in dailyMeal)
                {
                    var mealNutritions = _foodService.GetMealNutritionsAsync(meal, userFavouriteDtos);

                    dailyNutritions.AnimalProtein += mealNutritions.AnimalProtein;
                    dailyNutritions.PlantProtein += mealNutritions.PlantProtein;
                    dailyNutritions.Sugar += mealNutritions.Sugar;
                    dailyNutritions.Vegetables += mealNutritions.Vegetables;
                }

                result[dailyMeal.Key] = dailyNutritions;
            }

            return result;
        }

        public async Task InsertAsync(string userId, InsertTargetPayload payload, CancellationToken cancellationToken)
        {
            foreach (var answer in payload.Targets)
            {
                var target =
                    await _context.Targets.SingleOrDefaultAsync(x => x.Id == answer.TargetId, cancellationToken);

                if (target == null)
                {
                    throw new NotFoundException(nameof(Target), answer.TargetId);
                }

                var userTarget = await _context.UserTargets
                    .Where(x => x.UserId == userId && x.TargetId == answer.TargetId &&
                                x.Created > DateTime.Now.AddDays(-_statisticsPeriod)).OrderBy(x => x.Created)
                    .LastOrDefaultAsync(cancellationToken);

                if (userTarget == null)
                {
                    await _context.UserTargets.AddAsync(
                        new UserTarget
                            {UserId = userId, TargetId = answer.TargetId, TargetAnswerCode = answer.UserAnswerCode},
                        cancellationToken);
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

        public bool NewTriggered(StatisticsUserDto statisticsUserDto, string userId)
        {
            var activeTargets =
                statisticsUserDto.UserTargets.Any(x => x.Created > DateTime.Today.AddDays(-_statisticsPeriod));

            return !activeTargets && (Get(statisticsUserDto, userId, null)).Any();
        }

        public bool AnyAnswered(IEnumerable<UserTargetDto> userTargetDtos) =>
        userTargetDtos.Any(x => !string.IsNullOrEmpty(x.TargetAnswerCode));

        public  bool AnyActivated(IEnumerable<UserTargetDto> userTargets) =>
            userTargets.Any(x =>
                !string.IsNullOrEmpty(x.TargetAnswerCode) &&
                x.Created > DateTime.Now.AddDays(-_statisticsPeriod));

        public int GetDaysTillFirstEvaluationAsync(StatisticsUserDto user, CancellationToken cancellationToken)
        {
            if (user == null)
            {
                throw new NotFoundException(nameof(User), string.Empty);
            }

            if (AnyAnswered(user.UserTargets))
                return 0;

            var dateToCheck = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);

            var userCreatedDate = new DateTime(user.Created.Year, user.Created.Month, user.Created.Day);

            var daysSinceCreation = (dateToCheck - userCreatedDate).Days;

            var diaryRecords = user.Meals
                .Where(x =>  x.Date >= dateToCheck.AddDays(-_statisticsPeriod) &&
                            x.Date <= dateToCheck).ToList();

            var daysRecorded = diaryRecords.Select(x => x.Date)
                .Distinct()
                .Count();

            if (daysSinceCreation <= _statisticsPeriod - _statisticsMinimumDays)
                return _statisticsPeriod - daysSinceCreation;
            if (daysSinceCreation > _statisticsPeriod - _statisticsMinimumDays &&
                daysSinceCreation < _statisticsPeriod)
            {
                if (daysRecorded + _statisticsPeriod - daysSinceCreation >= _statisticsMinimumDays)
                {
                    return _statisticsPeriod - daysSinceCreation;
                }

                return _statisticsMinimumDays - daysRecorded;
            }

            if (daysRecorded >= _statisticsMinimumDays)
                return 0;
            for (int i = 1; i < _statisticsMinimumDays; i++)
            {
                daysRecorded = diaryRecords.Where(x =>
                        x.Date >= dateToCheck.AddDays(-_statisticsPeriod + i - 1))
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