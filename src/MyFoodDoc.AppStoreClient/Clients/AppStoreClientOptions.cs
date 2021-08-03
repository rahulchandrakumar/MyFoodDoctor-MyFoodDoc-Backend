using System;
using System.Collections.Generic;
using System.Text;

namespace MyFoodDoc.AppStoreClient.Clients
{
    public class AppStoreClientOptions
    {
        public string VerifyReceiptUrl { get; set; }

        public string VerifyReceiptSharedSecret { get; set; }

        public string SubscriptionProducts { get; set; }

        public string ZppSubscriptionProducts { get; set; }

        public string VerifyReceiptSandBoxUrl { get; set; }
    }
}
