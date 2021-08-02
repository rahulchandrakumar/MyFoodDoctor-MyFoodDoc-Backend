using System;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MyFoodDoc.Application.Enums;
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

        public async Task<ReceiptValidationResult> ValidateReceipt(SubscriptionType subscriptionType, string receiptData)
        {
            var content = await ValidateReceiptEnvironment(receiptData, _options.VerifyReceiptUrl);

            if (string.IsNullOrWhiteSpace(content))
            {
                _logger.LogError("AppStoreClient > ValidateReceipt > No valid response.");
                throw new Exception("AppStoreClient > ValidateReceipt > No valid response.");
            }

            var result = JsonConvert.DeserializeObject<Rootobject>(content);

            // workAround apple bug 
            if (result?.status == 21007)
            {
                content = await ValidateReceiptEnvironment(receiptData, _options.VerifyReceiptSandBoxUrl);

                if (string.IsNullOrWhiteSpace(content))
                {
                    _logger.LogError("AppStoreClient > ValidateReceipt > No valid response.");
                    throw new Exception("AppStoreClient > ValidateReceipt > No valid response.");
                }

                result = JsonConvert.DeserializeObject<Rootobject>(content);
            }


            if (result == null)
            {
                _logger.LogError($"Couldn't deserialize response: {content}");
                throw new Exception($"Couldn't deserialize response: {content}");
            }

            string[] productIds = (subscriptionType == SubscriptionType.MyFoodDoc) ?
                _options.SubscriptionProducts.Split(',') :  _options.ZppSubscriptionProducts.Split(',');

            if (result.latest_receipt_info == null || !result.latest_receipt_info.Any(x => productIds.Contains(x.product_id)))
            {
                _logger.LogError($"latest_receipt_info is missing: {content}");
                throw new Exception($"latest_receipt_info is missing: {content}");
            }

            var latestReceiptInfo = result.latest_receipt_info.First(x => productIds.Contains(x.product_id));

            DateTime ? purchaseDateDateTime = null;

            if (!string.IsNullOrEmpty(latestReceiptInfo.purchase_date))
                purchaseDateDateTime = DateTime.ParseExact(
                    latestReceiptInfo.purchase_date,
                    "yyyy-MM-dd HH:mm:ss 'Etc/GMT'",
                    System.Globalization.CultureInfo.InvariantCulture,
                    System.Globalization.DateTimeStyles.AssumeUniversal);

            DateTime? subscriptionExpirationDateTime = null;

            if (!string.IsNullOrEmpty(latestReceiptInfo.expires_date))
                subscriptionExpirationDateTime = DateTime.ParseExact(
                    latestReceiptInfo.expires_date,
                    "yyyy-MM-dd HH:mm:ss 'Etc/GMT'",
                    System.Globalization.CultureInfo.InvariantCulture,
                    System.Globalization.DateTimeStyles.AssumeUniversal);

            return new ReceiptValidationResult
            {
                PurchaseDate = purchaseDateDateTime,
                SubscriptionExpirationDate = subscriptionExpirationDateTime,
                ProductId = latestReceiptInfo.product_id,
                OriginalTransactionId = latestReceiptInfo.original_transaction_id
            };
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="subscriptionType"></param>
        /// <param name="receiptData"></param>
        /// <returns></returns>
        /// <remarks> <seealso cref="https://developer.apple.com/forums/thread/675893"/> </remarks>
        public async Task<string> ValidateReceiptEnvironment(string receiptData, string url)
        {
            var response = await _httpClient.PostAsync(url, new StringContent(JsonConvert.SerializeObject(new ReceiptValidationRequest() { Password = _options.VerifyReceiptSharedSecret, ExcludeOldTransactions = true, ReceiptData = receiptData }), Encoding.UTF8, "application/json"));

            if (!response.IsSuccessStatusCode)
            {
                _logger.LogError(response.StatusCode.ToString());
                throw new Exception("Failed to call app store validation endpoint.");
            }

            return await response.Content.ReadAsStringAsync();
        }
    }
}
