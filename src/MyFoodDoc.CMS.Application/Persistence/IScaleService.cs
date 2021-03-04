using MyFoodDoc.CMS.Application.Models;
using MyFoodDoc.CMS.Application.Persistence.Base;

namespace MyFoodDoc.CMS.Application.Persistence
{
    public interface IScaleService : IServiceBasePaginatedRead<ScaleModel, int>, IServiceBaseWrite<ScaleModel, int>
    {
    }
}
