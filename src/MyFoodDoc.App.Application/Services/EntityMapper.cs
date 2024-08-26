using System.Linq;
using MyFoodDoc.App.Application.Abstractions;
using MyFoodDoc.App.Application.Models;
using MyFoodDoc.Application.Entities;
using MyFoodDoc.Application.Entities.Diary;

namespace MyFoodDoc.App.Application.Services
{
    public class EntityMapper : IEntityMapper
    {
        public DiaryEntryDtoMeal ToDto(Meal entity)
        {
            return new DiaryEntryDtoMeal
            {
                Id = entity.Id,
                Time = entity.Time,
                Type = entity.Type,
                Mood = entity.Mood,
                Ingredients = entity.Ingredients.Select(ToDto).ToArray(),
                Favourites = entity.Favourites.Select(ToDto).ToArray()
            };
        }

        public DiaryEntryDtoMealIngredient ToDto(MealIngredient entity)
        {
            return new DiaryEntryDtoMealIngredient
            {
                Ingredient = ToDto(entity.Ingredient),
                Amount = entity.Amount
            };
        }

        public MealFavouriteDto ToDto(MealFavourite entity)
        {
            return new MealFavouriteDto
            {
                Favourite = ToDto(entity.Favourite)
            };
        }

        public FavouriteDto ToDto(Favourite entity)
        {
            return new FavouriteDto
            {
                Id = entity.Id,
                Title = entity.Title,
                Calories = entity.Ingredients.Sum(x => (x.Ingredient.Calories ?? x.Ingredient.CaloriesExternal) * x.Amount),
                Carbohydrate = entity.Ingredients.Sum(x => (x.Ingredient.Carbohydrate ?? x.Ingredient.CarbohydrateExternal) * x.Amount),
                Protein = entity.Ingredients.Sum(x => (x.Ingredient.Protein ?? x.Ingredient.ProteinExternal) * x.Amount),
                Fat = entity.Ingredients.Sum(x => (x.Ingredient.Fat ?? x.Ingredient.FatExternal) * x.Amount),
                SaturatedFat = entity.Ingredients.Sum(x => (x.Ingredient.SaturatedFat ?? x.Ingredient.SaturatedFatExternal) * x.Amount),
                PolyunsaturatedFat = entity.Ingredients.Sum(x => (x.Ingredient.PolyunsaturatedFat ?? x.Ingredient.PolyunsaturatedFatExternal) * x.Amount),
                MonounsaturatedFat = entity.Ingredients.Sum(x => (x.Ingredient.MonounsaturatedFat ?? x.Ingredient.MonounsaturatedFatExternal) * x.Amount),
                Cholesterol = entity.Ingredients.Sum(x => (x.Ingredient.Cholesterol ?? x.Ingredient.CholesterolExternal) * x.Amount),
                Sodium = entity.Ingredients.Sum(x => (x.Ingredient.Sodium ?? x.Ingredient.SodiumExternal) * x.Amount),
                Potassium = entity.Ingredients.Sum(x => (x.Ingredient.Potassium ?? x.Ingredient.PotassiumExternal) * x.Amount),
                Fiber = entity.Ingredients.Sum(x => (x.Ingredient.Fiber ?? x.Ingredient.FiberExternal) * x.Amount),
                Sugar = entity.Ingredients.Sum(x => (x.Ingredient.Sugar ?? x.Ingredient.SugarExternal) * x.Amount),
                Vegetables = entity.Ingredients.Sum(x => (x.Ingredient.Vegetables ?? 0) * x.Amount),
                Ingredients = entity.Ingredients.Select(ToDto).ToList()
            };
        }

        public FavouriteIngredientDto ToDto(FavouriteIngredient entity)
        {
            return new FavouriteIngredientDto
            {
                Ingredient = ToDto(entity.Ingredient),
                Amount = entity.Amount
            };
        }

        public IngredientDto ToDto(Ingredient entity)
        {
            return new IngredientDto
            {
                Id = entity.Id,
                FoodId = entity.FoodId,
                FoodName = entity.FoodName,
                ServingId = entity.ServingId,
                ServingDescription = entity.ServingDescription,
                MetricServingAmount = entity.MetricServingAmount,
                MetricServingUnit = entity.MetricServingUnit,
                MeasurementDescription = entity.MeasurementDescription,
                Calories = entity.Calories ?? entity.CaloriesExternal,
                Carbohydrate = entity.Carbohydrate ?? entity.CarbohydrateExternal,
                Protein = entity.Protein ?? entity.ProteinExternal,
                Fat = entity.Fat ?? entity.FatExternal,
                SaturatedFat = entity.SaturatedFat ?? entity.SaturatedFatExternal,
                PolyunsaturatedFat = entity.PolyunsaturatedFat ?? entity.PolyunsaturatedFatExternal,
                MonounsaturatedFat = entity.MonounsaturatedFat ?? entity.MonounsaturatedFatExternal,
                Cholesterol = entity.Cholesterol ?? entity.CholesterolExternal,
                Sodium = entity.Sodium ?? entity.SodiumExternal,
                Potassium = entity.Potassium ?? entity.PotassiumExternal,
                Fiber = entity.Fiber ?? entity.FiberExternal,
                Sugar = entity.Sugar ?? entity.SugarExternal,
            };
        }
    }
}
