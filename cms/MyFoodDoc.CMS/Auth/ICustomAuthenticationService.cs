using MyFoodDoc.CMS.Models;
using System.Threading.Tasks;

namespace MyFoodDoc.CMS.Auth
{
    public interface ICustomAuthenticationService
    {
        Task<AppUser> Login(string username, string password);
    }
}
