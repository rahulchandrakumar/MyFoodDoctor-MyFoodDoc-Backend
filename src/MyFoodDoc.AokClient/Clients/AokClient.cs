using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MyFoodDoc.AokClient.Abstractions;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

namespace MyFoodDoc.AokClient.Clients
{
    public class AokClient : IAokClient
    {
        private const string DateFormat = "yyyyMM-dd";

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
            var url = $"{_options.Address}/validate?insurance_number={insuranceNumber}&date_of_birth={birthday.ToString(DateFormat)}&source={source}";

            var response = await _httpClient.GetAsync(url).ConfigureAwait(false);

            if (!response.IsSuccessStatusCode)
            {
                if (response.StatusCode == HttpStatusCode.NotFound)
                {
                    return new ValidateResponse();
                }

                if (response.StatusCode == HttpStatusCode.UnprocessableEntity)
                {
                    var errorResponse = await GetUnprocessableEntityResponse(response);
                    var err = $"The call to AOK validate endpoint failed with errors: {string.Join(';', errorResponse.Errors)}";
                    _logger.LogError(err);
                    throw new Exception(err);
                }

                var error = $"Failed to call AOK validate endpoint. The reponse code is {response.StatusCode}";
                _logger.LogError(error);
                throw new Exception(error);
            }

            var content = await response.Content.ReadAsStringAsync();

            return JsonSerializer.Deserialize<ValidateResponse>(content);
        }

        public async Task<RegisterResponse> Register(RegisterRequest request)
        {
            var url = $"{_options.Address}/register";

            var parameters = new Dictionary<string, string>()
            {
                { "insurance_number", request.InsuranceNumber },
                { "date_of_birth", request.Birthday.ToString(DateFormat) },
                { "source", request.Source }
            };

            var formContent = new FormUrlEncodedContent(parameters);

            var response = await _httpClient.PostAsync(url, formContent).ConfigureAwait(false);

            if (!response.IsSuccessStatusCode)
            {
                if (response.StatusCode == System.Net.HttpStatusCode.UnprocessableEntity)
                {
                    var errorResponse = await GetUnprocessableEntityResponse(response);
                    var err = $"The call to AOK validate endpoint failed with errors: {string.Join(';', errorResponse.Errors)}";
                    _logger.LogError(err);
                    throw new Exception(err);
                }

                var error = $"Failed to call AOK register endpoint. The reponse code is {response.StatusCode}";
                _logger.LogError(error);
                throw new Exception(error);
            }

            var content = await response.Content.ReadAsStringAsync();

            return JsonSerializer.Deserialize<RegisterResponse>(content);
        }

        public async Task Delete(string token)
        {
            var url = $"{_options.Address}/delete";

            var parameters = new Dictionary<string, string>()
            {
                { "token", token }
            };

            var formContent = new FormUrlEncodedContent(parameters);

            var response = await _httpClient.PostAsync(url, formContent).ConfigureAwait(false);

            if (!response.IsSuccessStatusCode)
            {
                if (response.StatusCode == System.Net.HttpStatusCode.UnprocessableEntity)
                {
                    var errorResponse = await GetUnprocessableEntityResponse(response);
                    var err = $"The call to AOK validate endpoint failed with errors: {string.Join(';', errorResponse.Errors)}";
                    _logger.LogError(err);
                    throw new Exception(err);
                }

                var error = $"Failed to call AOK delete endpoint. The reponse code is {response.StatusCode}";
                _logger.LogError(error);
                throw new Exception(error);
            }
        }

        private async Task<UnprocessableEntityResponse> GetUnprocessableEntityResponse(HttpResponseMessage message)
        {
            var content = await message.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<UnprocessableEntityResponse>(content);
        }
    }
}
