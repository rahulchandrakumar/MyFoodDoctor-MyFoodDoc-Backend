using MyFoodDoc.App.Application.Models;
using MyFoodDoc.App.Application.Payloads.Target;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace MyFoodDoc.App.Application.Abstractions
{
    public interface ITargetService
    {
        Task<bool> NewTriggered(string userId, CancellationToken cancellationToken);
        Task<bool> AnyAnswered(string userId, CancellationToken cancellationToken);
        Task<ICollection<OptimizationAreaDto>> GetAsync(string userId, CancellationToken cancellationToken);
        Task InsertAsync(string userId, InsertTargetPayload payload, CancellationToken cancellationToken);
    }
}
