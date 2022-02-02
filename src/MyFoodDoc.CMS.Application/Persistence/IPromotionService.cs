using MyFoodDoc.CMS.Application.Models;
using MyFoodDoc.CMS.Application.Persistence.Base;
using System.Threading;
using System.Threading.Tasks;

namespace MyFoodDoc.CMS.Application.Persistence
{
    public interface IPromotionService : IServiceBasePaginatedRead<PromotionModel, int>, IServiceBaseWrite<PromotionModel, int>
    {
        Task<byte[]> GetCouponsFile(int Id, CancellationToken cancellationToken = default);
    }
}
