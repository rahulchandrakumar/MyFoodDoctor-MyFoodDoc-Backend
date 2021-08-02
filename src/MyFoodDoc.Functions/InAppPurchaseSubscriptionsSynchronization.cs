using Microsoft.Azure.WebJobs;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using MyFoodDoc.Application.Abstractions;
using MyFoodDoc.Application.Entities.Subscriptions;
using MyFoodDoc.Application.Enums;
using MyFoodDoc.AppStoreClient.Abstractions;
using MyFoodDoc.GooglePlayStoreClient.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace MyFoodDoc.Functions
{
    public class InAppPurchaseSubscriptionsSynchronization
    {
        private readonly IApplicationContext _context;
        private readonly IAppStoreClient _appStoreClient;
        private readonly IGooglePlayStoreClient _googlePlayStoreClient;

        public InAppPurchaseSubscriptionsSynchronization(
            IApplicationContext context,
            IAppStoreClient appStoreClient,
            IGooglePlayStoreClient googlePlayStoreClient)
        {
            _context = context;
            _appStoreClient = appStoreClient;
            _googlePlayStoreClient = googlePlayStoreClient;
        }

        [FunctionName("InAppPurchaseSubscriptionsSynchronization")]
        public async Task RunAsync(
            [TimerTrigger("0 */20 * * * *" /*"%TimerInterval%"*/, RunOnStartup = true)]
            TimerInfo myTimer,
            ILogger log,
            CancellationToken cancellationToken)
        {
            await AppStoreSubscriptionsSynchronization(log, cancellationToken);
            await GooglePlayStoreSubscriptionsSynchronization(log, cancellationToken);
        }

        private async Task AppStoreSubscriptionsSynchronization(ILogger log,
            CancellationToken cancellationToken)
        {
            var appStoreSubscriptions = await _context.AppStoreSubscriptions.OrderBy(x => x.LastSynchronized).Take(400).ToListAsync(cancellationToken);

            log.LogInformation($"{appStoreSubscriptions.Count} AppStore subscriptions to update.");

            int currentBatchCount = 0;
            int batchSize = 40;

            var appStoreSubscriptionsToUpdate = new List<AppStoreSubscription>();

            int errors = 0;

            if (appStoreSubscriptions?.Any() == true)
            {
                foreach (var appStoreSubscription in appStoreSubscriptions)
                {

                    try
                    {
                        var validateReceiptValidationResult = await _appStoreClient.ValidateReceipt(appStoreSubscription.Type, appStoreSubscription.ReceiptData);

                        appStoreSubscription.LastSynchronized = DateTime.Now;
                        appStoreSubscription.IsValid = appStoreSubscription.Type == SubscriptionType.MyFoodDoc ? validateReceiptValidationResult.SubscriptionExpirationDate > DateTime.Now : validateReceiptValidationResult.PurchaseDate.Value.AddYears(1) > DateTime.Now;
                        appStoreSubscription.ProductId = validateReceiptValidationResult.ProductId;
                        appStoreSubscription.OriginalTransactionId = validateReceiptValidationResult.OriginalTransactionId;
                    }
                    catch (Exception e)
                    {
                        log.LogError(e, $"Error on validating AppStore user '{appStoreSubscription.UserId}'. Message = {e.Message}. StackTrace = {e.StackTrace}");

                        errors++;

                        continue;
                    }

                    appStoreSubscriptionsToUpdate.Add(appStoreSubscription);
                    currentBatchCount++;

                    if (currentBatchCount == batchSize)
                    {
                        _context.AppStoreSubscriptions.UpdateRange(appStoreSubscriptionsToUpdate);

                        await _context.SaveChangesAsync(cancellationToken);

                        log.LogInformation($"{currentBatchCount} AppStore subscriptions updated.");

                        currentBatchCount = 0;
                        appStoreSubscriptionsToUpdate = new List<AppStoreSubscription>();
                    }
                }

                if (currentBatchCount > 0)
                {
                    _context.AppStoreSubscriptions.UpdateRange(appStoreSubscriptions);

                    await _context.SaveChangesAsync(cancellationToken);

                    log.LogInformation($"{currentBatchCount} AppStore subscriptions updated.");
                }

                if (errors > 0)
                    log.LogError($"{errors} AppStore subscriptions cause error on validation.");
            }
        }

        private async Task GooglePlayStoreSubscriptionsSynchronization(ILogger log,
            CancellationToken cancellationToken)
        {
            var googlePlayStoreSubscriptions = await _context.GooglePlayStoreSubscriptions.OrderBy(x => x.LastSynchronized).Take(400).ToListAsync(cancellationToken);

            log.LogInformation($"{googlePlayStoreSubscriptions.Count} GooglePlayStore subscriptions to update.");

            int currentBatchCount = 0;
            int batchSize = 40;

            var googlePlayStoreSubscriptionsToUpdate = new List<GooglePlayStoreSubscription>();

            int errors = 0;

            if (googlePlayStoreSubscriptions.Any())
            {
                foreach (var googlePlayStoreSubscription in googlePlayStoreSubscriptions)
                {
                    try
                    {
                        var validateReceiptValidationResult = await _googlePlayStoreClient.ValidatePurchase(googlePlayStoreSubscription.Type, googlePlayStoreSubscription.SubscriptionId, googlePlayStoreSubscription.PurchaseToken);

                        googlePlayStoreSubscription.LastSynchronized = DateTime.Now;
                        googlePlayStoreSubscription.IsValid = googlePlayStoreSubscription.Type == SubscriptionType.MyFoodDoc ?
                                        ((validateReceiptValidationResult.ExpirationDate != null && validateReceiptValidationResult.ExpirationDate.Value > DateTime.Now &&
                                          validateReceiptValidationResult.StartDate != null && validateReceiptValidationResult.StartDate.Value < DateTime.Now)
                                         || (validateReceiptValidationResult.AutoRenewing != null && validateReceiptValidationResult.AutoRenewing.Value &&
                                             validateReceiptValidationResult.ExpirationDate != null && validateReceiptValidationResult.ExpirationDate.Value < DateTime.Now)) : validateReceiptValidationResult.PurchaseDate.Value.AddYears(1) > DateTime.Now;

                    }
                    catch (Exception e)
                    {
                        log.LogError(e, $"Error on validating GooglePlayStore user '{googlePlayStoreSubscription.UserId}'. Message = {e.Message}. StackTrace = {e.StackTrace}");

                        errors++;

                        continue;
                    }

                    googlePlayStoreSubscriptionsToUpdate.Add(googlePlayStoreSubscription);
                    currentBatchCount++;

                    if (currentBatchCount == batchSize)
                    {
                        _context.GooglePlayStoreSubscriptions.UpdateRange(googlePlayStoreSubscriptionsToUpdate);

                        await _context.SaveChangesAsync(cancellationToken);

                        log.LogInformation($"{currentBatchCount} GooglePlayStore subscriptions updated.");

                        currentBatchCount = 0;
                        googlePlayStoreSubscriptionsToUpdate = new List<GooglePlayStoreSubscription>();
                    }
                }

                if (currentBatchCount > 0)
                {
                    _context.GooglePlayStoreSubscriptions.UpdateRange(googlePlayStoreSubscriptionsToUpdate);

                    await _context.SaveChangesAsync(cancellationToken);

                    log.LogInformation($"{currentBatchCount} GooglePlayStore subscriptions updated.");
                }

                if (errors > 0)
                    log.LogError($"{errors} GooglePlayStore subscriptions cause error on validation.");
            }
        }
    }
}
