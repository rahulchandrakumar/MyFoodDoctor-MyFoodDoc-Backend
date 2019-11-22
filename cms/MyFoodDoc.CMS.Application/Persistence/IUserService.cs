using MyFoodDoc.CMS.Application.Models;
using MyFoodDoc.CMS.Application.Persistence.Base;

namespace MyFoodDoc.CMS.Application.Persistence
{
    public interface IUserService: IServiceBaseRead<UserModel, int>, IServiceBaseWrite<UserModel, int>
    {
    }
}
