using MyFoodDoc.CMS.Application.Models;
using MyFoodDoc.CMS.Application.Persistence.Base;

namespace MyFoodDoc.CMS.Application.Persistence
{
    public interface IWebViewService: IServiceBaseRead<WebViewModel, int>, IServiceBaseWrite<WebViewModel, int>
    {
    }
}
