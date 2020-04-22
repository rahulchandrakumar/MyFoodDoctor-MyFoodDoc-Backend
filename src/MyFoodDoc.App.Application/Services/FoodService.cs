using System.Collections.Generic;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using MyFoodDoc.App.Application.Abstractions;
using MyFoodDoc.App.Application.Models;
using MyFoodDoc.Application.Abstractions;
using MyFoodDoc.Application.Entites;
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
                Protein = 0,
                Sugar = 0
            };

            foreach (var mealIngredient in _context.MealIngredients.Where(x => x.MealId == mealId))
            {
                var ingredient = await _context.Ingredients.SingleAsync(x => x.Id == mealIngredient.IngredientId);

                var cacheKey = cachePrefix + ingredient.FoodId;

                var food = await _cache.GetOrCreateAsync(cacheKey, async entry =>
                {
                    entry.AbsoluteExpirationRelativeToNow = TimeSpan.FromSeconds(300);

                    var result = await _fatSecretClient.GetFoodAsync(ingredient.FoodId);

                    return result;
                });

                var serving = food.Servings.Serving.Single(x => x.Id == ingredient.ServingId);

                result.Protein += (ingredient.Protein ?? serving.Protein) * mealIngredient.Amount;
                result.Sugar += (ingredient.Sugar ?? serving.Sugar) * mealIngredient.Amount;
            }

            return result;
        }
    }
}
