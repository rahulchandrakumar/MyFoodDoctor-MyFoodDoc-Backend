﻿using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using MyFoodDoc.App.Application.Abstractions;
using MyFoodDoc.App.Application.Models;
using MyFoodDoc.Application.Abstractions;
using MyFoodDoc.Application.Entities.Diary;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace MyFoodDoc.App.Application.Services
{
    public class FoodService : IFoodService
    {
        private readonly IApplicationContext _context;
        private readonly IMapper _mapper;

        public FoodService(IApplicationContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<ICollection<IngredientDto>> GetFoodAsync(long foodId, CancellationToken cancellationToken)
        {
            var ingredients = await _context.Ingredients
                .Where(x => x.FoodId == foodId)
                .ProjectTo<IngredientDto>(_mapper.ConfigurationProvider).ToListAsync(cancellationToken);

            return ingredients;
        }

        public async Task<MealNutritionsDto> GetMealNutritionsAsync(int mealId, CancellationToken cancellationToken)
        {
            var result = new MealNutritionsDto
            {
                AnimalProtein = 0,
                PlantProtein = 0,
                Calories = 0,
                Sugar = 0,
                Vegetables = 0,
                Fiber = 0,
                Meals = 1
            };

            foreach (var mealIngredient in await _context.MealIngredients
                .Include(x => x.Ingredient)
                .Where(x => x.MealId == mealId)
                .ToListAsync(cancellationToken))
            {
                decimal protein = (mealIngredient.Ingredient.Protein ?? mealIngredient.Ingredient.ProteinExternal) * mealIngredient.Amount;

                if (mealIngredient.Ingredient.ContainsPlantProtein)
                    result.PlantProtein += protein;
                else
                    result.AnimalProtein += protein;

                result.Calories += (mealIngredient.Ingredient.Calories ?? mealIngredient.Ingredient.CaloriesExternal) * mealIngredient.Amount;
                result.Sugar += (mealIngredient.Ingredient.Sugar ?? mealIngredient.Ingredient.SugarExternal) * mealIngredient.Amount;
                result.Vegetables += (mealIngredient.Ingredient.Vegetables ?? 0) * mealIngredient.Amount;
                result.Fiber += (mealIngredient.Ingredient.Fiber ?? mealIngredient.Ingredient.FiberExternal) * mealIngredient.Amount;
            }

            foreach (var mealFavourite in await _context.MealFavourites
                .Where(x => x.MealId == mealId)
                .ToListAsync(cancellationToken))
            {
                foreach (var favouriteIngredient in await _context.FavouriteIngredients
                    .Include(x => x.Ingredient)
                    .Where(x => x.FavouriteId == mealFavourite.FavouriteId)
                    .ToListAsync(cancellationToken))
                {
                    decimal protein = (favouriteIngredient.Ingredient.Protein ?? favouriteIngredient.Ingredient.ProteinExternal) * favouriteIngredient.Amount;

                    if (favouriteIngredient.Ingredient.ContainsPlantProtein)
                        result.PlantProtein += protein;
                    else
                        result.AnimalProtein += protein;

                    result.Calories += (favouriteIngredient.Ingredient.Calories ?? favouriteIngredient.Ingredient.CaloriesExternal) * favouriteIngredient.Amount;
                    result.Sugar += (favouriteIngredient.Ingredient.Sugar ?? favouriteIngredient.Ingredient.SugarExternal) * favouriteIngredient.Amount;
                    result.Vegetables += (favouriteIngredient.Ingredient.Vegetables ?? 0) * favouriteIngredient.Amount;
                    result.Fiber += (favouriteIngredient.Ingredient.Fiber ?? favouriteIngredient.Ingredient.FiberExternal) * favouriteIngredient.Amount;
                }
            }

            return result;
        }
    }
}
