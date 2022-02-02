using IdentityModel.Client;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MyFoodDoc.FatSecretClient.Abstractions;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace MyFoodDoc.FatSecretClient.Clients
{
    public class FatSecretIdentityServerClient : IFatSecretIdentityServerClient
    {
        private readonly HttpClient _httpClient;
        private readonly FatSecretIdentityServerClientOptions _options;
        private readonly ILogger<FatSecretIdentityServerClient> _logger;

        public FatSecretIdentityServerClient(
            HttpClient httpClient,
            IOptions<FatSecretIdentityServerClientOptions> options,
            ILogger<FatSecretIdentityServerClient> logger)
        {
            _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
            _options = options.Value;
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<TokenResponse> RequestClientCredentialsTokenAsync()
        {
            var clientCredentialsTokenRequest = new ClientCredentialsTokenRequest
            {
                Address = _options.Address + "/connect/token",
                GrantType = _options.GrantType,
                ClientId = _options.ClientId,
                ClientSecret = _options.ClientSecret,
                Scope = _options.Scope
            };

            var tokenResponse = await _httpClient.RequestClientCredentialsTokenAsync(clientCredentialsTokenRequest);

            if (tokenResponse.IsError)
            {
                _logger.LogError(tokenResponse.Error);
                throw new HttpRequestException(tokenResponse.Error);
            }

            return tokenResponse;
        }
    }
}
