using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using MyFoodDoc.App.Application.Abstractions;
using MyFoodDoc.App.Application.Configuration;
using MyFoodDoc.App.Application.Models;
using MyFoodDoc.App.Application.Payloads.Target;
using MyFoodDoc.Application.Abstractions;
using MyFoodDoc.Application.Entites;
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
        private readonly int _statisticsPeriod;

        public TargetService(IApplicationContext context, IFoodService foodService, IOptions<StatisticsOptions> statisticsOptions)
        {
            _context = context;
            _foodService = foodService;
            _statisticsPeriod = statisticsOptions.Value.Period > 0 ? statisticsOptions.Value.Period : 7;
        }

        public async Task<ICollection<OptimizationAreaDto>> GetAsync(string userId, CancellationToken cancellationToken)
        {
            var result = new List<OptimizationAreaDto>();

            var userDiets = await _context.UserDiets.Where(x => x.UserId == userId).Select(x => x.DietId).ToListAsync();
            var userIndications = await _context.UserIndications.Where(x => x.UserId == userId).Select(x => x.IndicationId).ToListAsync();
            var userMotivations = await _context.UserMotivations.Where(x => x.UserId == userId).Select(x => x.MotivationId).ToListAsync();

            var targetIds = _context.DietTargets.Where(x => userDiets.Contains(x.DietId)).Select(x => x.TargetId)
                .Union(_context.IndicationTargets.Where(x => userIndications.Contains(x.IndicationId)).Select(x => x.TargetId))
                .Union(_context.MotivationTargets.Where(x => userMotivations.Contains(x.MotivationId)).Select(x => x.TargetId)).Distinct();

            if (!targetIds.Any())
                return result;

            var dailyUserIngredients = new Dictionary<DateTime, MealNutritionsDto>();

            foreach (var dailyMeals in _context.Meals
                .Where(x => x.UserId == userId && x.Date > DateTime.Now.AddDays(-7)).ToList().GroupBy(g => g.Date))
            {
                var dailyNutritions = new MealNutritionsDto
                {
                    Protein = 0,
                    Sugar = 0
                };

                foreach (var meal in dailyMeals)
                {
                    var mealNutritions = await _foodService.GetMealNutritionsAsync(meal.Id, cancellationToken);

                    dailyNutritions.Protein += mealNutritions.Protein;
                    dailyNutritions.Sugar += mealNutritions.Sugar;
                }

                dailyUserIngredients[dailyMeals.Key] = dailyNutritions;
            }

            if (!dailyUserIngredients.Any())
                return result;

            var triggeredTargets = new List<TargetDto>();       

            foreach (var target in _context.Targets.Include(x => x.OptimizationArea).Where(x => targetIds.Contains(x.Id)).OrderBy(x => x.Priority))
            {
                int triggeredDaysCount = 0;
                decimal bestValue = 0;

                //TODO: use constants or enums
                if (target.OptimizationArea.Key == "protein")
                {
                    if (target.TriggerOperator == TriggerOperator.GreaterThan)
                    {
                        var triggeredDays = dailyUserIngredients.Values.Where(x => x.Protein > target.TriggerValue);

                        triggeredDaysCount = triggeredDays.Count();

                        if (triggeredDays.Any())
                            bestValue = triggeredDays.Min(x => x.Protein);
                    }
                    else
                    {
                        var triggeredDays = dailyUserIngredients.Values.Where(x => x.Protein < target.TriggerValue);

                        triggeredDaysCount = triggeredDays.Count();

                        if (triggeredDays.Any())
                            bestValue = triggeredDays.Max(x => x.Protein);
                    }
                }
                else if (target.OptimizationArea.Key == "sugar")
                {
                    if (target.TriggerOperator == TriggerOperator.GreaterThan)
                    {
                        var triggeredDays = dailyUserIngredients.Values.Where(x => x.Sugar > target.TriggerValue);

                        triggeredDaysCount = triggeredDays.Count();

                        if (triggeredDays.Any())
                            bestValue = triggeredDays.Min(x => x.Sugar);
                    }
                    else
                    {
                        var triggeredDays = dailyUserIngredients.Values.Where(x => x.Sugar < target.TriggerValue);

                        triggeredDaysCount = triggeredDays.Count();

                        if (triggeredDays.Any())
                            bestValue = triggeredDays.Max(x => x.Sugar);
                    }
                }

                var frequency = (decimal)triggeredDaysCount * 100 / _statisticsPeriod;

                if (frequency > target.Threshold)
                {
                    var targetImage = _context.Images.SingleOrDefault(x => x.Id == target.ImageId);

                    var targetDto = new TargetDto
                    {
                        Id = target.Id,
                        Type = target.Type.ToString(),
                        Title = target.Title,
                        Text = target.Text,
                        ImageUrl = targetImage?.Url
                    };

                    //TODO: use constants or enums
                    if (target.Type == TargetType.Adjustment)
                    {
                        var adjustmentTarget = await _context.AdjustmentTargets.SingleAsync(x => x.TargetId == target.Id);

                        decimal recommendedValue = (adjustmentTarget.StepDirection == AdjustmentTargetStepDirection.Ascending)
                            ? bestValue - bestValue % adjustmentTarget.Step + adjustmentTarget.Step
                            : bestValue - bestValue % adjustmentTarget.Step;

                        targetDto.Answers = new List<TargetAnswerDto>();

                        if (recommendedValue != adjustmentTarget.TargetValue)
                            targetDto.Answers.Add(new TargetAnswerDto { Code = "recommended", Value = string.Format(adjustmentTarget.RecommendedText, Math.Round(recommendedValue)) });

                        targetDto.Answers.Add(new TargetAnswerDto { Code = "target", Value = adjustmentTarget.TargetText });
                        targetDto.Answers.Add(new TargetAnswerDto { Code = "remain", Value = adjustmentTarget.RemainText });
                    }
                    else
                    {
                        targetDto.Answers = new[] {
                                new TargetAnswerDto { Code = "yes", Value ="Ja"},
                                new TargetAnswerDto { Code = "no", Value = "Nein" }
                            };
                    }

                    var userAnswer = await _context.UserTargets.SingleOrDefaultAsync(x => x.UserId == userId && x.TargetId == target.Id);

                    targetDto.UserAnswerCode = userAnswer?.TargetAnswerCode;

                    if (!result.Any(x => x.Key == target.OptimizationArea.Key))
                    {
                        var optimizationAreaImage = _context.Images.SingleOrDefault(x => x.Id == target.OptimizationArea.ImageId);

                        result.Add(new OptimizationAreaDto
                        {
                            Key = target.OptimizationArea.Key,
                            Name = target.OptimizationArea.Name,
                            Text = target.OptimizationArea.Text,
                            ImageUrl = optimizationAreaImage?.Url,
                            Targets = new List<TargetDto>()
                        });
                    }

                    result.Single(x => x.Key == target.OptimizationArea.Key).Targets.Add(targetDto);
                }
            }

            return result;
        }
         
        public async Task InsertAsync(string userId, InsertTargetPayload payload, CancellationToken cancellationToken)
        {
            foreach(var answer in payload.Targets)
            {
                var userTarget = _context.UserTargets.SingleOrDefault(x => x.UserId == userId && x.TargetId == answer.TargetId);

                if(userTarget == null)
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
    }
}
