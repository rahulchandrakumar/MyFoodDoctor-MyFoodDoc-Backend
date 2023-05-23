using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using MyFoodDoc.App.Application.Abstractions.V2;
using MyFoodDoc.App.Application.Models;
using MyFoodDoc.App.Application.Models.StatisticsDto;
using MyFoodDoc.Application.Abstractions;

namespace MyFoodDoc.App.Application.Services.V2
{
    public class FoodServiceV2 : IFoodServiceV2
    {
        private readonly IApplicationContext _context;
        private readonly IMapper _mapper;

        public FoodServiceV2(IApplicationContext context, IMapper mapper)
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

        public MealNutritionsDto GetMealNutritionsAsync(
        MealDto meal,
         UserFavouriteDto userFavouriteDto)
        {
            var result = new MealNutritionsDto
            {
                AnimalProtein = 0,
                PlantProtein = 0,
                Calories = 0,
                Sugar = 0,
                Vegetables = 0,
                Meals = 1
            };

            foreach (var mealIngredient in meal.IngredientDtos)
            {
                var protein = (mealIngredient.Protein ?? mealIngredient.ProteinExternal) * mealIngredient.Amount;

                if (!mealIngredient.ContainsPlantProtein)
                    result.AnimalProtein += protein;
                else
                {
                    result.PlantProtein += protein;
                }

                result.Calories += (mealIngredient.Calories ?? mealIngredient.CaloriesExternal) * mealIngredient.Amount;
                result.Sugar += (mealIngredient.Sugar ?? mealIngredient.SugarExternal) * mealIngredient.Amount;
                result.Vegetables += (mealIngredient.Vegetables ?? 0) * mealIngredient.Amount;
            }

            foreach (var mealFavouriteId in meal.MealFavouriteIds)
            {
                var ingredients = userFavouriteDto.Ingredient;
                
                foreach (var favouriteIngredient in ingredients)
                {
                    decimal protein = (favouriteIngredient.Protein ?? favouriteIngredient.ProteinExternal) * favouriteIngredient.Amount;

                    if (favouriteIngredient.ContainsPlantProtein)
                        result.PlantProtein += protein;
                    else
                        result.AnimalProtein += protein;

                    result.Calories += (favouriteIngredient.Calories ?? favouriteIngredient.CaloriesExternal) * favouriteIngredient.Amount;
                    result.Sugar += (favouriteIngredient.Sugar ?? favouriteIngredient.SugarExternal) * favouriteIngredient.Amount;
                    result.Vegetables += (favouriteIngredient.Vegetables ?? 0) * favouriteIngredient.Amount;
                }
            }

            return result;
        }
    }
}
