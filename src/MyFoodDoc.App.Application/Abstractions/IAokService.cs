using MyFoodDoc.App.Application.Payloads.Aok;
using System.Threading;
using System.Threading.Tasks;

namespace MyFoodDoc.App.Application.Abstractions
{
    public interface IAokService
    {
        Task<bool> ExistsAsync(string userId);
        Task InsertUserAsync(string userId, AokUserPayload aokUserPayload, CancellationToken cancellationToken);
    }
}
