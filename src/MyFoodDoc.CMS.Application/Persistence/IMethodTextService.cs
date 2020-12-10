using MyFoodDoc.CMS.Application.Models;
using MyFoodDoc.CMS.Application.Persistence.Base;

namespace MyFoodDoc.CMS.Application.Persistence
{
    public interface IMethodTextService : IServiceBasePaginatedChildrenRead<MethodTextModel, int, int>, IServiceBaseWrite<MethodTextModel, int>
    {
    }
}
