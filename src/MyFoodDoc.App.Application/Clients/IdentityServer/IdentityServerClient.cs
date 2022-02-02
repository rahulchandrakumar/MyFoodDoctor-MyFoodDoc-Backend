using IdentityModel.Client;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace MyFoodDoc.App.Application.Clients.IdentityServer
{
    public class IdentityServerClient : IIdentityServerClient
    {
        private readonly HttpClient _httpClient;
        private readonly IdentityServerClientOptions _options;
        private readonly ILogger<IdentityServerClient> _logger;

        public IdentityServerClient(
            HttpClient httpClient,
            IOptions<IdentityServerClientOptions> options,
            ILogger<IdentityServerClient> logger)
        {
            _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
            _options = options.Value;
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<TokenResponse> RequestPasswordTokenAsync(string username, string password)
        {
            // request the access token
            var passwordTokenRequest = new PasswordTokenRequest
            {
                Address = _options.Address + "/connect/token",
                ClientId = _options.ClientId,
                Scope = _options.Scope,
                UserName = username,
                Password = password
            };

            return await _httpClient.RequestPasswordTokenAsync(passwordTokenRequest);
        }
    }
}
