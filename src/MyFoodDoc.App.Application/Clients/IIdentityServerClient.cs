using IdentityModel.Client;
using System.Threading.Tasks;

namespace MyFoodDoc.App.Application.Clients
{
    public interface IIdentityServerClient
    {
        Task<TokenResponse> RequestPasswordTokenAsync(string username, string password);
    }
}
