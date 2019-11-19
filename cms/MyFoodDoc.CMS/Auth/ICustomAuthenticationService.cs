using MyFoodDoc.CMS.Models;

namespace MyFoodDoc.CMS.Auth
{
    public interface ICustomAuthenticationService
    {
        AppUser Login(string username, string password);
    }
}
