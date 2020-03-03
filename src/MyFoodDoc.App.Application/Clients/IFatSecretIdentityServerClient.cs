using System.Threading.Tasks;
using IdentityModel.Client;

namespace MyFoodDoc.App.Application.Clients
{
    public interface IFatSecretIdentityServerClient
    {
        Task<TokenResponse> RequestClientCredentialsTokenAsync();
    }
}
