using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using IdentityModel.Client;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;

namespace MyFoodDoc.App.Application.Clients.FatSecret
{
    public class FatSecretClient : IFatSecretClient
    {
        private readonly HttpClient _httpClient;
        private readonly FatSecretClientOptions _options;
        private readonly IFatSecretIdentityServerClient _fatSecretIdentityServerClient;
        private readonly ILogger<FatSecretClient> _logger;

        public FatSecretClient(HttpClient httpClient,
            IOptions<FatSecretClientOptions> options,
            IFatSecretIdentityServerClient fatSecretIdentityServerClient,
            ILogger<FatSecretClient> logger)
        {
            _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
            _options = options.Value;
            _fatSecretIdentityServerClient = fatSecretIdentityServerClient
                                             ?? throw new ArgumentNullException(nameof(fatSecretIdentityServerClient));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<Food> GetFoodAsync(long id)
        {
            var accessToken = await _fatSecretIdentityServerClient.RequestClientCredentialsTokenAsync();
            _httpClient.SetBearerToken(accessToken.AccessToken);

            // request data from our Protected API
            var response = await _httpClient.GetAsync($"?method=food.get&format=json&food_id={id}");
            if (!response.IsSuccessStatusCode)
            {
                _logger.LogError(response.StatusCode.ToString());
                throw new Exception("Failed to get protected resources.");
            }

            var content = await response.Content.ReadAsStringAsync();

            var result = JsonConvert.DeserializeObject<GetFoodResult>(content).Food;

            if (result == null)
            {
                _logger.LogWarning($"Couldn't deserialize response: {content}");
            }

            return result;
        }
    }
}
