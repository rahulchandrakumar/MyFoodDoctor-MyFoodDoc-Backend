using System;
using System.Threading.Tasks;
using Google.Apis.AndroidPublisher.v3;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Services;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MyFoodDoc.GooglePlayStoreClient.Abstractions;
using Newtonsoft.Json;

namespace MyFoodDoc.GooglePlayStoreClient.Clients
{
    public class GooglePlayStoreClient : IGooglePlayStoreClient
    {
        private readonly GooglePlayStoreClientOptions _options;
        private readonly ILogger<GooglePlayStoreClient> _logger;

        public GooglePlayStoreClient(IOptions<GooglePlayStoreClientOptions> options,
            ILogger<GooglePlayStoreClient> logger)
        {
            _options = options.Value;
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<PurchaseValidationResult> ValidatePurchase(string subscriptionId, string purchaseToken)
        {
            //Azure key vault adds extra slash before new line
            _options.Certificate.PrivateKey = _options.Certificate.PrivateKey.Replace("\\n", "\n");

            string jsonCertificate = JsonConvert.SerializeObject(_options.Certificate);

            try
            {
                var publisherService = new AndroidPublisherService(new BaseClientService.Initializer
                {
                    HttpClientInitializer = GoogleCredential
                        .FromJson(jsonCertificate)
                        .CreateScoped(AndroidPublisherService.Scope.Androidpublisher)
                        .UnderlyingCredential,
                    ApplicationName = "MyFoodDoc"
                });

                var request = publisherService.Purchases.Subscriptions.Get(
                    _options.PackageName,
                    subscriptionId,
                    purchaseToken
                );

                var response = await request.ExecuteAsync().ConfigureAwait(false);

                return new PurchaseValidationResult()
                {
                    StartDate =  response.StartTimeMillis == null
                        ? (DateTime?)null
                        : DateTimeOffset.FromUnixTimeMilliseconds(response.StartTimeMillis.Value).DateTime,
                    ExpirationDate = response.ExpiryTimeMillis == null
                        ? (DateTime?) null
                        : DateTimeOffset.FromUnixTimeMilliseconds(response.ExpiryTimeMillis.Value).DateTime,
                    AutoRenewing = response.AutoRenewing,
                    CancelReason = response.CancelReason,
                    LinkedPurchaseToken = response.LinkedPurchaseToken
                };
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Error on Google play in-app purchase validation");
                throw;
            }
        }
    }
}
