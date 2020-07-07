using MyFoodDoc.CMS.Application.Models;
using MyFoodDoc.CMS.Application.Persistence.Base;

namespace MyFoodDoc.CMS.Application.Persistence
{
    public interface ISubchapterService : IServiceBasePaginatedChildrenRead<SubchapterModel, int, int>, IServiceBaseWrite<SubchapterModel, int>
    {
    }
}
