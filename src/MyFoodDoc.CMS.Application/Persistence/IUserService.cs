using MyFoodDoc.CMS.Application.Models;
using MyFoodDoc.CMS.Application.Persistence.Base;
using System.Threading;
using System.Threading.Tasks;

namespace MyFoodDoc.CMS.Application.Persistence
{
    public interface IUserService: IServiceBasePaginatedRead<UserModel, int>, IServiceBaseWrite<UserModel, int>
    {
        Task<UserModel> GetByUsername(string userName, CancellationToken cancellationToken = default);
    }
}
