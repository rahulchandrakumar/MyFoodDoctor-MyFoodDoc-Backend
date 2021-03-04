using MyFoodDoc.CMS.Application.Models;
using MyFoodDoc.CMS.Application.Persistence.Base;

namespace MyFoodDoc.CMS.Application.Persistence
{
    public interface IChoiceService : IServiceBasePaginatedChildrenRead<ChoiceModel, int, int>, IServiceBaseWrite<ChoiceModel, int>
    {
    }
}
