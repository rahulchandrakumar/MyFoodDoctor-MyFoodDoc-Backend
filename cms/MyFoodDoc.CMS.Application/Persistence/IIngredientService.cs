using MyFoodDoc.CMS.Application.Models;
using MyFoodDoc.CMS.Application.Persistence.Base;

namespace MyFoodDoc.CMS.Application.Persistence
{
    public interface IIngredientService : IServiceBaseRead<IngredientModel, int>, IServiceBaseWrite<IngredientModel, int>
    {
    }
}
