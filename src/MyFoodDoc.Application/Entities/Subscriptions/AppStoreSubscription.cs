namespace MyFoodDoc.Application.Entities.Subscriptions
{
    public class AppStoreSubscription : AbstractSubscription
    {
        public string ReceiptData { get; set; }

        public string ProductId { get; set; }

        public string OriginalTransactionId { get; set; }
    }
}
