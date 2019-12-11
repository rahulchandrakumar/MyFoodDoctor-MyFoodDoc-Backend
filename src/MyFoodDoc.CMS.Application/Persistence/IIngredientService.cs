using MyFoodDoc.CMS.Application.FilterModels;
using MyFoodDoc.CMS.Application.Models;
using MyFoodDoc.CMS.Application.Persistence.Base;

namespace MyFoodDoc.CMS.Application.Persistence
{
    public interface IIngredientService : IServiceBasePaginatedRead<IngredientModel, int, IngredientFilter>, IServiceBaseWrite<IngredientModel, int>
    {
    }
}
