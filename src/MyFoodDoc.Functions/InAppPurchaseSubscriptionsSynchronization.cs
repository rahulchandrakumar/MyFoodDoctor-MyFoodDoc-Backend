﻿using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using MyFoodDoc.Application.Abstractions;
using MyFoodDoc.Application.Enums;
using MyFoodDoc.AppStoreClient.Abstractions;
using MyFoodDoc.GooglePlayStoreClient.Abstractions;

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
            [TimerTrigger("0 */30 * * * *" /*"%TimerInterval%"*/, RunOnStartup = true)]
            TimerInfo myTimer,
            ILogger log,
            CancellationToken cancellationToken)
        {
            var users = await _context.Users.Where(x => x.SubscriptionUpdated != null).ToListAsync(cancellationToken);

            log.LogInformation($"{users.Count()} records to update.");

            int updated = 0;
            int inconsistent = 0;

            if (users.Any())
            {
                foreach (var user in users)
                {
                    if (user.SubscriptionType == SubscriptionType.AppStore && !string.IsNullOrEmpty(user.ReceiptData))
                    {
                        var validateReceiptValidationResult = await _appStoreClient.ValidateReceipt(user.ReceiptData);

                        user.ProductId = validateReceiptValidationResult.ProductId;
                        user.OriginalTransactionId = validateReceiptValidationResult.OriginalTransactionId;
                        user.HasValidSubscription = validateReceiptValidationResult.SubscriptionExpirationDate > DateTime.Now;
                        user.SubscriptionUpdated = DateTime.Now;
                    }
                    else if (user.SubscriptionType == SubscriptionType.GooglePlayStore && !string.IsNullOrEmpty(user.SubscriptionId) && !string.IsNullOrEmpty(user.PurchaseToken))
                    {
                        var validateReceiptValidationResult = await _googlePlayStoreClient.ValidatePurchase(user.SubscriptionId, user.PurchaseToken);

                        user.HasValidSubscription = validateReceiptValidationResult.CancelReason == null &&
                                                    ((validateReceiptValidationResult.ExpirationDate != null && validateReceiptValidationResult.ExpirationDate.Value > DateTime.Now &&
                                                      validateReceiptValidationResult.StartDate != null && validateReceiptValidationResult.StartDate.Value < DateTime.Now)
                                                     || (validateReceiptValidationResult.AutoRenewing != null && validateReceiptValidationResult.AutoRenewing.Value &&
                                                         validateReceiptValidationResult.ExpirationDate != null && validateReceiptValidationResult.ExpirationDate.Value < DateTime.Now));
                        user.SubscriptionUpdated = DateTime.Now;
                    }
                    else
                    {
                        inconsistent++;
                        continue;
                    }

                    _context.Users.Update(user);

                    updated++;
                }

                await _context.SaveChangesAsync(cancellationToken);

                log.LogInformation($"{updated} records updated.");

                if (inconsistent > 0)
                    log.LogInformation($"{inconsistent} records are inconsistent.");
            }
        }
    }
}