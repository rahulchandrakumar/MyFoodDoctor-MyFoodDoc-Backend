using MyFoodDoc.Api.Models;
using System.Threading.Tasks;

namespace MyFoodDoc.App.Application.Abstractions
{
    public interface IFoodService
    {
        Task<Ingredient> GetAll(string q);
    }
}
