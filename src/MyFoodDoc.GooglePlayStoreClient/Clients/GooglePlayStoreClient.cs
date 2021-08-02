using System;
using System.Threading.Tasks;
using Google.Apis.AndroidPublisher.v3;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Services;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MyFoodDoc.Application.Enums;
using MyFoodDoc.GooglePlayStoreClient.Abstractions;
using Newtonsoft.Json;
using SendGrid;

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

        public async Task<PurchaseValidationResult> ValidatePurchase(SubscriptionType subscriptionType, string subscriptionId, string purchaseToken)
        {
            //Azure key vault adds extra slash before new line
            _options.Certificate.PrivateKey = _options.Certificate.PrivateKey.Replace("\\n", "\n");

            string jsonCertificate = JsonConvert.SerializeObject(_options.Certificate);

            var publisherService = new AndroidPublisherService(new BaseClientService.Initializer
            {
                HttpClientInitializer = GoogleCredential
                    .FromJson(jsonCertificate)
                    .CreateScoped(AndroidPublisherService.Scope.Androidpublisher)
                    .UnderlyingCredential,
                ApplicationName = "MyFoodDoc"
            });

            if (subscriptionType == SubscriptionType.MyFoodDoc)
            {
                var request = publisherService.Purchases.Subscriptions.Get(
                    _options.PackageName,
                    subscriptionId,
                    purchaseToken
                );

                try
                {
                    var response = await request.ExecuteAsync();

                    return new PurchaseValidationResult()
                    {
                        StartDate = response.StartTimeMillis == null
                            ? (DateTime?)null
                            : DateTimeOffset.FromUnixTimeMilliseconds(response.StartTimeMillis.Value).LocalDateTime,
                        ExpirationDate = response.ExpiryTimeMillis == null
                            ? (DateTime?)null
                            : DateTimeOffset.FromUnixTimeMilliseconds(response.ExpiryTimeMillis.Value).LocalDateTime,
                        AutoRenewing = response.AutoRenewing,
                        CancelReason = response.CancelReason,
                        LinkedPurchaseToken = response.LinkedPurchaseToken
                    };
                }
                catch (Google.GoogleApiException googleEx)
                {
                    _logger.LogError(googleEx, "Error GooglePlayStoreClient > ValidatePurchase > Subscriptions validation");

                    if (googleEx.Error.Code == 410) // The subscription purchase is no longer available for query because it has been expired for too long
                    {
                        return new PurchaseValidationResult();
                    }

                    throw;
                }
                catch (Exception e)
                {
                    _logger.LogError(e, "Error GooglePlayStoreClient > ValidatePurchase > Subscriptions validation");

                    throw;
                }
            }
            else
            {
                var request = publisherService.Purchases.Products.Get(
                    _options.PackageName,
                    subscriptionId,
                    purchaseToken
                );

                try
                {
                    var response = await request.ExecuteAsync();

                    return new PurchaseValidationResult()
                    {
                        PurchaseDate = response.PurchaseTimeMillis == null
                                        ? (DateTime?)null
                                        : DateTimeOffset.FromUnixTimeMilliseconds(response.PurchaseTimeMillis.Value).LocalDateTime
                    };
                }
                catch (Google.GoogleApiException googleEx)
                {
                    _logger.LogError(googleEx, "Error GooglePlayStoreClient > ValidatePurchase > Products validation");

                    if (googleEx.Error.Code == 401) // The subscription purchase is no longer available for query because it has been expired for too long
                    {
                        return new PurchaseValidationResult();
                    }

                    throw;
                }
                catch (Exception e)
                {
                    _logger.LogError(e, "Error GooglePlayStoreClient > ValidatePurchase > Products validation");

                    throw;
                }
            }

        }
    }
}
