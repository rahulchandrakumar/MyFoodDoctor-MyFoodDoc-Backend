using Microsoft.Azure.WebJobs;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using MyFoodDoc.Application.Abstractions;
using MyFoodDoc.Application.Entities.Subscriptions;
using MyFoodDoc.Application.Enums;
using MyFoodDoc.AppStoreClient.Abstractions;
using MyFoodDoc.AppStoreClient.Clients;
using MyFoodDoc.GooglePlayStoreClient.Abstractions;
using MyFoodDoc.GooglePlayStoreClient.Clients;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace MyFoodDoc.Functions.Functions
{
    public class InAppPurchaseSubscriptionsSynchronization
    {
        private readonly IApplicationContext _context;
        private readonly IAppStoreClient _appStoreClient;
        private readonly IGooglePlayStoreClient _googlePlayStoreClient;
        private const int BatchSize = 200;

        public InAppPurchaseSubscriptionsSynchronization(
            IApplicationContext context,
            IAppStoreClient appStoreClient,
            IGooglePlayStoreClient googlePlayStoreClient)
        {
            _context = context;
            _appStoreClient = appStoreClient;
            _googlePlayStoreClient = googlePlayStoreClient;
        }

        [FunctionName(nameof(InAppPurchaseSubscriptionsSynchronization))]
        public async Task RunAsync(
            [TimerTrigger("0 */10 * * * *" /*"%TimerInterval%"*/, RunOnStartup = false)] TimerInfo myTimer,
            ILogger log)
        {
            if (myTimer.IsPastDue)
            {
                log.LogInformation("Timer is running late!");
            }

            log.LogInformation($"InAppPurchaseSubscriptionsSynchronization executed at: {DateTime.Now}");

            await AppStoreSubscriptionsSynchronization(log, CancellationToken.None);

            await GooglePlayStoreSubscriptionsSynchronization(log, CancellationToken.None);
        }

        private async Task AppStoreSubscriptionsSynchronization(ILogger log,
            CancellationToken cancellationToken)
        {
            var appStoreSubscriptions = await _context.AppStoreSubscriptions
               .AsNoTracking()
               .Where(
               x =>
               x.LastSynchronized < DateTime.Now.AddHours(-10) // HACK: do not take same register at least 10 hours
               && (
               x.FirstSynchronized == null || x.FirstSynchronized > DateTime.Now.AddYears(-2))) // HACK: do not consider registers older than 1 year
               .OrderBy(x => x.LastSynchronized)
               .Take(BatchSize)
               .ToListAsync(cancellationToken);

            var receiptsTasks = appStoreSubscriptions.Select(x => CallApple(x));

            var receipts = await Task.WhenAll(receiptsTasks);
            var receiptIds = receipts
                .Where(x => x.receiptValidationResult != null)
                .Select(x => x.id);

            var appStoreSubscriptionsUpdate = appStoreSubscriptions.Where(x => receiptIds.Contains(x.Id));

            foreach (var appStoreSubscription in appStoreSubscriptionsUpdate)
            {
                var validateReceiptValidationResult = receipts.SingleOrDefault(x => x.id == appStoreSubscription.Id).receiptValidationResult;

                appStoreSubscription.LastSynchronized = DateTime.Now;
                appStoreSubscription.IsValid = appStoreSubscription.Type == SubscriptionType.MyFoodDoc ? validateReceiptValidationResult.SubscriptionExpirationDate > DateTime.Now : validateReceiptValidationResult.PurchaseDate.Value.AddYears(1) > DateTime.Now;
                appStoreSubscription.ProductId = validateReceiptValidationResult.ProductId;
                appStoreSubscription.OriginalTransactionId = validateReceiptValidationResult.OriginalTransactionId;

                if (appStoreSubscription.FirstSynchronized is null)
                    appStoreSubscription.FirstSynchronized = DateTime.Now;
            }

            _context.AppStoreSubscriptions.UpdateRange(appStoreSubscriptionsUpdate);
            await _context.SaveChangesAsync(cancellationToken);

            log.LogInformation($"{appStoreSubscriptionsUpdate.Count()} AppStore subscriptions updated.");

            if (receiptIds.Count() < receipts.Count())
                log.LogError($"{receipts.Count() - receiptIds.Count()} AppStore subscriptions cause error on validation.");

            async Task<(int id, ReceiptValidationResult receiptValidationResult)> CallApple(AppStoreSubscription appStoreSubscription)
            {
                try
                {
                    return (appStoreSubscription.Id, await _appStoreClient.ValidateReceipt(appStoreSubscription.Type, appStoreSubscription.ReceiptData));
                }
                catch (Exception e)
                {
                    log.LogError(e, $"Error on validating AppStore user '{appStoreSubscription.UserId}'. Message = {e.Message}. StackTrace = {e.StackTrace}");
                    return (appStoreSubscription.Id, null);
                }
            }
        }

        private async Task GooglePlayStoreSubscriptionsSynchronization(ILogger log,
            CancellationToken cancellationToken)
        {
            var googlePlayStoreSubscriptions = await _context.GooglePlayStoreSubscriptions
                .AsNoTracking()
                .Where(x => x.IsExpired == false
                && x.LastSynchronized < DateTime.Now.AddHours(-10) // HACK: do not take same register at least 10 hours
                && (x.FirstSynchronized == null || x.FirstSynchronized > DateTime.Now.AddYears(-2))) // HACK: do not consider registers older than 1 year
                .OrderBy(x => x.LastSynchronized)
                .Take(BatchSize)
                .ToListAsync(cancellationToken);

            var receiptsTasks = googlePlayStoreSubscriptions.Select(x => CallGoogle(x));

            var receipts = await Task.WhenAll(receiptsTasks);
            var receiptIds = receipts
                .Where(x => x.purchaseValidationResult != null)
                .Select(x => x.id);

            var googlePlayStoreSubscriptionsUpdate = googlePlayStoreSubscriptions.Where(x => receiptIds.Contains(x.Id));

            foreach (var googlePlayStoreSubscription in googlePlayStoreSubscriptionsUpdate)
            {
                var validateReceiptValidationResult = receipts.SingleOrDefault(x => x.id == googlePlayStoreSubscription.Id).purchaseValidationResult;

                if (validateReceiptValidationResult.LinkedPurchaseToken == "410")
                {
                    googlePlayStoreSubscription.LastSynchronized = DateTime.Now;
                    googlePlayStoreSubscription.IsValid = false;
                    googlePlayStoreSubscription.IsExpired = true;
                }
                else
                {
                    googlePlayStoreSubscription.LastSynchronized = DateTime.Now;
                    googlePlayStoreSubscription.IsValid = googlePlayStoreSubscription.Type == SubscriptionType.MyFoodDoc ?
                                    ((validateReceiptValidationResult.ExpirationDate != null && validateReceiptValidationResult.ExpirationDate.Value > DateTime.Now &&
                                      validateReceiptValidationResult.StartDate != null && validateReceiptValidationResult.StartDate.Value < DateTime.Now)
                                     || (validateReceiptValidationResult.AutoRenewing != null && validateReceiptValidationResult.AutoRenewing.Value &&
                                         validateReceiptValidationResult.ExpirationDate != null && validateReceiptValidationResult.ExpirationDate.Value < DateTime.Now)) : validateReceiptValidationResult.PurchaseDate.Value.AddYears(1) > DateTime.Now;
                }

                if (googlePlayStoreSubscription.FirstSynchronized is null)
                    googlePlayStoreSubscription.FirstSynchronized = DateTime.Now;
            }

            _context.GooglePlayStoreSubscriptions.UpdateRange(googlePlayStoreSubscriptionsUpdate);
            await _context.SaveChangesAsync(cancellationToken);

            log.LogInformation($"{googlePlayStoreSubscriptionsUpdate.Count()} GooglePlayStore subscriptions updated.");

            if (receiptIds.Count() < receipts.Count())
                log.LogError($"{receipts.Count() - receiptIds.Count()} AppStore subscriptions cause error on validation.");

            async Task<(int id, PurchaseValidationResult purchaseValidationResult)> CallGoogle(GooglePlayStoreSubscription googlePlayStoreSubscription)
            {
                try
                {
                    return (googlePlayStoreSubscription.Id, await _googlePlayStoreClient.ValidatePurchase(googlePlayStoreSubscription.Type, googlePlayStoreSubscription.SubscriptionId, googlePlayStoreSubscription.PurchaseToken));
                }
                catch (Google.GoogleApiException googleEx)
                {
                    log.LogError(googleEx, $"Error GooglePlayStoreSubscriptionsSynchronization > Validation > type: {googlePlayStoreSubscription.Type} subs: {googlePlayStoreSubscription.SubscriptionId} token: {googlePlayStoreSubscription.PurchaseToken}.  ErrorMessage = {googleEx.Message}");

                    if (googleEx.Error.Code == 410) // The subscription purchase is no longer available for query because it has been expired for too long
                    {
                        return (googlePlayStoreSubscription.Id, new PurchaseValidationResult
                        {
                            ExpirationDate = DateTime.Now,
                            LinkedPurchaseToken = "410"
                        });
                    }

                    return (googlePlayStoreSubscription.Id, null);
                }
                catch (Exception e)
                {
                    log.LogError(e, $"Error on validating GooglePlayStore user '{googlePlayStoreSubscription.UserId}'. Message = {e.Message}. StackTrace = {e.StackTrace}");

                    return (googlePlayStoreSubscription.Id, null);
                }
            }
        }
    }
}
