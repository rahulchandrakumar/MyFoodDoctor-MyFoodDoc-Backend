using System;
using System.Linq;
using System.Linq.Expressions;
using MyFoodDoc.App.Application.Models.StatisticsDto;
using MyFoodDoc.Application.Entities;

namespace MyFoodDoc.App.Application.Exceptions;

public static class Expressions
{
    public static Expression<Func<User, StatisticsUserDto>> Selector()
    {
        return x => new StatisticsUserDto()
        {
            Created = x.Created,
            Email = x.Email,
            Age = x.Birthday == null ? null : DateTime.UtcNow.Year - x.Birthday.Value.Year,
            Gender = x.Gender,
            Height = x.Height,
            InsuranceId = x.InsuranceId,
            Motivations = x.Motivations.SelectMany(m => m.Motivation.Targets.Select(mt => mt.TargetId)),
            Indications = x.Indications
            .SelectMany(i => i.Indication.Targets.Select(it => it.TargetId)),
            Diets = x.Diets.Select(d => new DietDto(
                d.DietId,
                d.Diet.Key,
                d.Diet.Targets.Select(dt => dt.TargetId)
            )).ToList(),
            Weights = x.WeightHistory.Select(w => new UserWeightDto(
                w.Date,
                w.Value
            )).ToList(),
            Meals = x.Meals.Select(m => new MealDto(
                m.Date,
                m.Type,
                m.Ingredients.Select(mi => new MealIngredientDto(mi.Ingredient.Protein,
                    mi.Ingredient.ProteinExternal,
                    mi.Ingredient.ContainsPlantProtein,
                    mi.Ingredient.Calories,
                    mi.Ingredient.CaloriesExternal,
                    mi.Ingredient.Sugar,
                    mi.Ingredient.SugarExternal,
                    mi.Ingredient.Vegetables,
                    mi.Amount)),
                m.Favourites.Select(mf => mf.FavouriteId)
            )).ToList(),
            
            UserTargets = x.UserTargets.Select(u =>
                new UserTargetDto(u.TargetId, u.TargetAnswerCode, u.Created, new FullUserTargetDto(
                    u.Target.Id,
                    new OptimizationAreaTargetDto(
                        u.Target.OptimizationArea.Type,
                        u.Target.OptimizationArea.ImageId,
                        u.Target.OptimizationArea.Key,
                        u.Target.OptimizationArea.Name,
                        u.Target.OptimizationArea.Text,
                        u.Target.OptimizationArea.LineGraphUpperLimit,
                        u.Target.OptimizationArea.LineGraphLowerLimit,
                        u.Target.OptimizationArea.LineGraphOptimal,
                        u.Target.OptimizationArea.AboveOptimalLineGraphTitle,
                        u.Target.OptimizationArea.AboveOptimalLineGraphText,
                        u.Target.OptimizationArea.BelowOptimalLineGraphTitle,
                        u.Target.OptimizationArea.BelowOptimalLineGraphText,
                        u.Target.OptimizationArea.OptimalLineGraphTitle,
                        u.Target.OptimizationArea.OptimalLineGraphText,
                        u.Target.OptimizationArea.AboveOptimalPieChartTitle,
                        u.Target.OptimizationArea.AboveOptimalPieChartText,
                        u.Target.OptimizationArea.BelowOptimalPieChartTitle,
                        u.Target.OptimizationArea.BelowOptimalPieChartText,
                        u.Target.OptimizationArea.OptimalPieChartTitle,
                        u.Target.OptimizationArea.OptimalPieChartText),
                    u.Target.TriggerOperator,
                    u.Target.TriggerValue,
                    u.Target.Threshold,
                    u.Target.Priority,
                    u.Target.Title,
                    u.Target.Text,
                    u.Target.Type,
                    u.Target.Image.Url,
                    ( new AdjustmentTargetDto(
                        u.Target.AdjustmentTargets.FirstOrDefault().TargetId,
                        u.Target.AdjustmentTargets.FirstOrDefault().StepDirection,
                        u.Target.AdjustmentTargets.FirstOrDefault().Step,
                        u.Target.AdjustmentTargets.FirstOrDefault().TargetValue,
                        u.Target.AdjustmentTargets.FirstOrDefault().RecommendedText,
                        u.Target.AdjustmentTargets.FirstOrDefault().TargetText,
                        u.Target.AdjustmentTargets.FirstOrDefault().RemainText)
                    )
                ))).ToList(),
            FavouriteIngredientDtos = new UserFavouriteDto(
                x.Favourites.FirstOrDefault().Id,
                x.Favourites.FirstOrDefault().Ingredients.Select(i => new FavouriteMealIngredientDto(
                    i.Ingredient.Protein,
                    i.Ingredient.ProteinExternal,
                    i.Ingredient.ContainsPlantProtein,
                    i.Ingredient.Calories,
                    i.Ingredient.CaloriesExternal,
                    i.Ingredient.Sugar,
                    i.Ingredient.SugarExternal,
                    i.Ingredient.Vegetables,
                    i.Amount)))
        };
    }
}