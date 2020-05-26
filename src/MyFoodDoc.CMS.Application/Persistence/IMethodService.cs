using MyFoodDoc.CMS.Application.Models;
using MyFoodDoc.CMS.Application.Persistence.Base;

namespace MyFoodDoc.CMS.Application.Persistence
{
    public interface IMethodService : IServiceBasePaginatedChildrenRead<MethodModel, int, int>, IServiceBaseWrite<MethodModel, int>
    {
    }
}
