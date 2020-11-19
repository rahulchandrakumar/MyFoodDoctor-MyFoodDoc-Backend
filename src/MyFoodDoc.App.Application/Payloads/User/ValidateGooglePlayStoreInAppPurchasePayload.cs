using System;
using System.Collections.Generic;
using System.Text;

namespace MyFoodDoc.App.Application.Payloads.User
{
    public class ValidateGooglePlayStoreInAppPurchasePayload
    {
        public string SubscriptionId { get; set; }
        public string PurchaseToken { get; set; }
    }
}
