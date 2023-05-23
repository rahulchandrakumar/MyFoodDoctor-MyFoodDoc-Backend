using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using MyFoodDoc.App.Application.Models;
using MyFoodDoc.App.Application.Models.StatisticsDto;

namespace MyFoodDoc.App.Application.Abstractions.V2
{
    public interface IFoodServiceV2
    {
        Task<ICollection<IngredientDto>> GetFoodAsync(long foodId, CancellationToken cancellationToken);

        MealNutritionsDto GetMealNutritionsAsync(MealDto meal, 
        UserFavouriteDto userFavouriteDto);
    }
}
