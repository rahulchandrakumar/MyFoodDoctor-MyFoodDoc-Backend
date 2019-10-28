using IdentityModel.Client;
using System.Threading.Tasks;

namespace MyFoodDoc.App.Application.Helpers
{
    public interface IIdentityServerClient
    {
        Task<TokenResponse> RequestPasswordTokenAsync(string username, string password);
    }
}
