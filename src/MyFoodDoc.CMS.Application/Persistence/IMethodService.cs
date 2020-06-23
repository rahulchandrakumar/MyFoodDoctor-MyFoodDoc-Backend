using MyFoodDoc.CMS.Application.Models;
using MyFoodDoc.CMS.Application.Persistence.Base;

namespace MyFoodDoc.CMS.Application.Persistence
{
    public interface IMethodService : IServiceBasePaginatedRead<MethodModel, int>, IServiceBaseWrite<MethodModel, int>
    {
    }
}
