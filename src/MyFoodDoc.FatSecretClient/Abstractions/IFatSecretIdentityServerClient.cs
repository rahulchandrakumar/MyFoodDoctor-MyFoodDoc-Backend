using IdentityModel.Client;
using System.Threading.Tasks;

namespace MyFoodDoc.FatSecretClient.Abstractions
{
    public interface IFatSecretIdentityServerClient
    {
        Task<TokenResponse> RequestClientCredentialsTokenAsync();
    }
}
