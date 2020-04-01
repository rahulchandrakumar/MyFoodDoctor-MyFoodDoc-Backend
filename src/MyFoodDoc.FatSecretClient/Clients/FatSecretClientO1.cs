using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MyFoodDoc.FatSecretClient.Abstractions;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web;
using System.Threading.Tasks;

namespace MyFoodDoc.FatSecretClient.Clients
{
    public class FatSecretClientO1 : IFatSecretClient
    {
        private readonly HttpClient _httpClient;
        private readonly FatSecretClientOptions _options;
        private readonly ILogger<FatSecretClientO1> _logger;

        public FatSecretClientO1(HttpClient httpClient,
            IOptions<FatSecretClientOptions> options,
            ILogger<FatSecretClientO1> logger)
        {
            _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
            _options = options.Value;
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<Food> GetFoodAsync(long id)
        {
            string outUrl;
            string outParms;
            var baseUri = new Uri(_options.Address + $"?method=food.get&format=json&food_id={id}&region=DE");

            //var baseUri = new Uri(_options.Address + $"?barcode=0852684003071&format=json&method=food.find_id_for_barcode");

            OAuthBase oAuth = new OAuthBase();

            var sig = oAuth.GenerateSignature(baseUri, _options.ConsumerKey, _options.ConsumerSecret, "GET", null, null, out outUrl, out outParms);

            var requestUrl = String.Format("{0}?{1}&oauth_signature={2}", outUrl, outParms, HttpUtility.UrlEncode(sig));

            var response = await _httpClient.GetAsync(requestUrl);
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
