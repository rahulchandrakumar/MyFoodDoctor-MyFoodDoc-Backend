using System.Collections.Generic;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using MyFoodDoc.App.Application.Abstractions;
using MyFoodDoc.App.Application.Models;
using MyFoodDoc.Application.Abstractions;
using MyFoodDoc.Application.Entities;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MyFoodDoc.App.Application.Clients;
using MyFoodDoc.App.Application.Exceptions;
using MyFoodDoc.FatSecretClient.Abstractions;
using System;
using Microsoft.Extensions.Caching.Memory;

namespace MyFoodDoc.App.Application.Services
{
    public class FoodService : IFoodService
    {
        private readonly IApplicationContext _context;
        private readonly IMapper _mapper;
        private readonly IFatSecretClient _fatSecretClient;
        private readonly IMemoryCache _cache;
        private const string cachePrefix = nameof(FoodService) + "_";

        public FoodService(IApplicationContext context, IMapper mapper, IFatSecretClient fatSecretClient, IMemoryCache cache)
        {
            _context = context;
            _mapper = mapper;
            _fatSecretClient = fatSecretClient;
            _cache = cache;
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
                Sugar = 0,
                Vegetables = 0
            };

            foreach (var mealIngredient in _context.MealIngredients.Include(x => x.Ingredient).Where(x => x.MealId == mealId))
            {
                var cacheKey = cachePrefix + mealIngredient.Ingredient.FoodId;

                var food = await _cache.GetOrCreateAsync(cacheKey, async entry =>
                {
                    entry.AbsoluteExpirationRelativeToNow = TimeSpan.FromSeconds(300);

                    var result = await _fatSecretClient.GetFoodAsync(mealIngredient.Ingredient.FoodId);

                    return result;
                });

                var serving = food.Servings.Serving.Single(x => x.Id == mealIngredient.Ingredient.ServingId);

                decimal protein = (mealIngredient.Ingredient.Protein ?? serving.Protein) * mealIngredient.Amount;

                if (mealIngredient.Ingredient.ContainsPlantProtein)
                    result.PlantProtein += protein;
                else
                    result.AnimalProtein += protein;

                result.Sugar += (mealIngredient.Ingredient.Sugar ?? serving.Sugar) * mealIngredient.Amount;
                result.Vegetables += (mealIngredient.Ingredient.Vegetables ?? 0) * mealIngredient.Amount;
            }

            return result;
        }
    }
}
