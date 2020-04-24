using MyFoodDoc.CMS.Application.Models;
using MyFoodDoc.CMS.Application.Persistence.Base;

namespace MyFoodDoc.CMS.Application.Persistence
{
    public interface ITargetService : IServiceBasePaginatedRead<TargetModel, int>, IServiceBaseWrite<TargetModel, int>
    {
    }
}
