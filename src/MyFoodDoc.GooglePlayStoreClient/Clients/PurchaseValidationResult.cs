using System;

namespace MyFoodDoc.GooglePlayStoreClient.Clients
{
    public class PurchaseValidationResult
    {
        public DateTime? PurchaseDate { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? ExpirationDate { get; set; }
        public bool? AutoRenewing { get; set; }
        public int? CancelReason { get; set; }
        public string LinkedPurchaseToken { get; set; }
    }
}
