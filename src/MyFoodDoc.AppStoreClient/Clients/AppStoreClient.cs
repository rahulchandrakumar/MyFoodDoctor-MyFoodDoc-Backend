using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MyFoodDoc.AppStoreClient.Abstractions;
using Newtonsoft.Json;

namespace MyFoodDoc.AppStoreClient.Clients
{
    public class AppStoreClient : IAppStoreClient
    {
        private readonly HttpClient _httpClient;
        private readonly AppStoreClientOptions _options;
        private readonly ILogger<AppStoreClient> _logger;

        public AppStoreClient(HttpClient httpClient,
            IOptions<AppStoreClientOptions> options,
            ILogger<AppStoreClient> logger)
        {
            _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
            _options = options.Value;
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<ReceiptValidationResult> ValidateReceipt(string receiptData)
        {
            var response = await _httpClient.PostAsync(_options.VerifyReceiptUrl, new StringContent(JsonConvert.SerializeObject(new ReceiptValidationRequest(){Password = _options.VerifyReceiptSharedSecret, ExcludeOldTransactions = true, ReceiptData = receiptData}), Encoding.UTF8, "application/json"));

            if (!response.IsSuccessStatusCode)
            {
                _logger.LogError(response.StatusCode.ToString());
                throw new Exception("Failed to call app store validation endpoint.");
            }

            var content = await response.Content.ReadAsStringAsync();

            var result = JsonConvert.DeserializeObject<Rootobject>(content);

            if (result == null)
            {
                _logger.LogError($"Couldn't deserialize response: {content}");
                throw new Exception($"Couldn't deserialize response: {content}");
            }

            if (result.latest_receipt_info == null || !result.latest_receipt_info.Any())
            {
                _logger.LogError($"latest_receipt_info is missing: {content}");
                throw new Exception($"latest_receipt_info is missing: {content}");
            }

            DateTime subscriptionExpirationDateTime = DateTime.ParseExact(
                result.latest_receipt_info[0].expires_date,
                "yyyy-MM-dd HH:mm:ss 'Etc/GMT'",
                System.Globalization.CultureInfo.InvariantCulture,
                System.Globalization.DateTimeStyles.AssumeUniversal);

            return new ReceiptValidationResult
            {
                SubscriptionExpirationDate = subscriptionExpirationDateTime
            };
        }
    }
}
