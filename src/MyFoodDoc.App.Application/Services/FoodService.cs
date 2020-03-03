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
using MyFoodDoc.App.Application.Clients.FatSecret;
using MyFoodDoc.App.Application.Exceptions;

namespace MyFoodDoc.App.Application.Services
{
    public class FoodService : IFoodService
    {
        private readonly IApplicationContext _context;
        private readonly IMapper _mapper;
        private readonly IFatSecretClient _fatSecretClient;

        public FoodService(IApplicationContext context, IMapper mapper, IFatSecretClient fatSecretClient)
        {
            _context = context;
            _mapper = mapper;
            _fatSecretClient = fatSecretClient;
        }

        public async Task<IngredientDto> GetAsync(int id, CancellationToken cancellationToken)
        {
            var ingredient = await _context.Ingredients
                .Where(x => x.Id == id)
                .ProjectTo<IngredientDto>(_mapper.ConfigurationProvider)
                .SingleOrDefaultAsync(cancellationToken);

            if (ingredient == null)
            {
                throw new NotFoundException(nameof(Ingredient), id);
            }

            //TODO: Add/use memory cache for nutritions

            var food = await _fatSecretClient.GetFoodAsync(ingredient.FoodId);

            if (food == null)
            {
                throw new NotFoundException(nameof(Food), ingredient.FoodId);
            }

            var serving = food.Servings.Serving.SingleOrDefault(s => s.Id == ingredient.ServingId);

            if (serving == null)
            {
                throw new NotFoundException(nameof(Serving), ingredient.ServingId);
            }

            if (ingredient.Calories == null)
                ingredient.Calories = serving.Calories;

            if (ingredient.Carbohydrate == null)
                ingredient.Carbohydrate = serving.Carbohydrate;

            if (ingredient.Protein == null)
                ingredient.Protein = serving.Protein;

            if (ingredient.Fat == null)
                ingredient.Fat = serving.Fat;

            if (ingredient.SaturatedFat == null)
                ingredient.SaturatedFat = serving.SaturatedFat;

            if (ingredient.PolyunsaturatedFat == null)
                ingredient.PolyunsaturatedFat = serving.PolyunsaturatedFat;

            if (ingredient.MonounsaturatedFat == null)
                ingredient.MonounsaturatedFat = serving.MonounsaturatedFat;

            if (ingredient.Cholesterol == null)
                ingredient.Cholesterol = serving.Cholesterol;

            if (ingredient.Sodium == null)
                ingredient.Sodium = serving.Sodium;

            if (ingredient.Potassium == null)
                ingredient.Potassium = serving.Potassium;

            if (ingredient.Fiber == null)
                ingredient.Fiber = serving.Fiber;

            if (ingredient.Sugar == null)
                ingredient.Sugar = serving.Sugar;

            return ingredient;
        }
    }
}
