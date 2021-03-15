using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MyFoodDoc.AokClient.Abstractions;
using System.Text.Json;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace MyFoodDoc.AokClient.Clients
{
    public class AokClient : IAokClient
    {
        private readonly HttpClient _httpClient;
        private readonly AokClientOptions _options;
        private readonly ILogger _logger;

        public AokClient(HttpClient httpClient,
            IOptions<AokClientOptions> options,
            ILogger<AokClient> logger)
        {
            _httpClient = httpClient;
            _options = options.Value;
            _logger = logger;
        }

        public async Task<ValidateResponse> ValidateAsync(string insuranceNumber, DateTime birthday, string source = "myfooddock")
        {
            var url = $"{_options.Address}/validate?insurance_number={insuranceNumber}&date_of_birth={birthday.ToString("yyyyMM-dd")}&source={source}";

            var response = await _httpClient.GetAsync(url).ConfigureAwait(false);

            if(!response.IsSuccessStatusCode)
            {
                var error = $"Failed to call AOK validate endpoint. The reponse code is {response.StatusCode}";
                _logger.LogError(error);
                throw new Exception(error);
            }

            var content = await response.Content.ReadAsStringAsync();
            
            return JsonSerializer.Deserialize<ValidateResponse>(content);
        }
    }
}
