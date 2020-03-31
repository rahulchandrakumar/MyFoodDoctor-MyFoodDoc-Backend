using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using IdentityModel.Client;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MyFoodDoc.FatSecretClient.Abstractions;
using Newtonsoft.Json;

namespace MyFoodDoc.FatSecretClient.Clients
{
    public class FatSecretClientO2 : IFatSecretClient
    {
        private readonly HttpClient _httpClient;
        private readonly FatSecretClientOptions _options;
        private readonly IFatSecretIdentityServerClient _fatSecretIdentityServerClient;
        private readonly ILogger<FatSecretClientO2> _logger;

        public FatSecretClientO2(HttpClient httpClient,
            IOptions<FatSecretClientOptions> options,
            IFatSecretIdentityServerClient fatSecretIdentityServerClient,
            ILogger<FatSecretClientO2> logger)
        {
            _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
            _options = options.Value;
            _fatSecretIdentityServerClient = fatSecretIdentityServerClient
                                             ?? throw new ArgumentNullException(nameof(fatSecretIdentityServerClient));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<Food> GetFoodAsync(long id)
        {
            //TODO: Use SessionHandler
            var accessToken = await _fatSecretIdentityServerClient.RequestClientCredentialsTokenAsync();
            _httpClient.SetBearerToken(accessToken.AccessToken);

            var response = await _httpClient.GetAsync($"?method=food.get&format=json&food_id={id}");
            if (!response.IsSuccessStatusCode)
            {
                _logger.LogError(response.StatusCode.ToString());
                throw new Exception("Failed to get resources.");
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
