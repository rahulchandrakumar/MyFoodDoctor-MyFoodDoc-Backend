using MyFoodDoc.App.Application.Models;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace MyFoodDoc.App.Application.Abstractions
{
    public interface IFoodService
    {
        Task<ICollection<IngredientDto>> GetFoodAsync(long foodId, CancellationToken cancellationToken);

        Task<MealNutritionsDto> GetMealNutritionsAsync(int mealId, CancellationToken cancellationToken);
    }
}
