namespace MyFoodDoc.Application.Entities.Subscriptions
{
    public class GooglePlayStoreSubscription : AbstractSubscription
    {
        public string SubscriptionId { get; set; }

        public string PurchaseToken { get; set; }
    }
}
