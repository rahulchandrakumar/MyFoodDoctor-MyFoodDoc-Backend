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
                Sugar = 0,
                Vegetables = 0
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

                result.Sugar += (mealIngredient.Ingredient.Sugar ?? mealIngredient.Ingredient.SugarExternal) * mealIngredient.Amount;
                result.Vegetables += (mealIngredient.Ingredient.Vegetables ?? 0) * mealIngredient.Amount;
            }

            return result;
        }
    }
}
