using System;
using System.Linq;
using System.Linq.Expressions;
using MyFoodDoc.App.Application.Models.StatisticsDto;
using MyFoodDoc.Application.Entities;

namespace MyFoodDoc.App.Application.Exceptions;

public static class Exprestions
{
    public static Expression<Func<User, StatisticsUserDto>> Selector()
    {
        return x => new StatisticsUserDto()
        {
            Created = x.Created,
            Email = x.Email,
            Age = x.Birthday == null ? null : (int?) (DateTime.UtcNow.Year - x.Birthday.Value.Year),
            Gender = x.Gender,
            Height = x.Height,
            InsuranceId = x.InsuranceId,
            Motivations = x.Motivations.Select(m => new MotivationDto(
                m.MotivationId,
      
                m.Motivation
                    .Targets
                    .Select(mt => mt.TargetId)
            )).ToList(),
            Indications = x.Indications.Select(i => new IndicationDto(
                i.IndicationId,
   
                i.Indication.Targets.Select(it => it.TargetId)
            )).ToList(),
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

            // UserTargets = x.UserTargets.Select(ut => new UserTargetDto(
            //     ut.TargetId,
            //     ut.TargetAnswerCode,
            //     ut.Created)
            // ).ToList()
        };
    }
}