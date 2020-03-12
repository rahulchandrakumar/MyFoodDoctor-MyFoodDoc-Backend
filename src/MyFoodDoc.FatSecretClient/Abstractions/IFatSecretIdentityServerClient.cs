using System.Threading.Tasks;
using IdentityModel.Client;

namespace MyFoodDoc.FatSecretClient.Abstractions
{
    public interface IFatSecretIdentityServerClient
    {
        Task<TokenResponse> RequestClientCredentialsTokenAsync();
    }
}
