using MyFoodDoc.App.Application.Models;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace MyFoodDoc.App.Application.Abstractions
{
    public interface IFoodService
    {
        Task<ICollection<IngredientDto>> GetAllAsync(string query, CancellationToken cancellationTokenl);
    }
}
