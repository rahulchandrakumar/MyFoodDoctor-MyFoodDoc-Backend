namespace MyFoodDoc.Application.Entities.Subscriptions
{
    public class AppStoreSubscription : AbstractSubscription
    {
        public string SubscriptionId { get; set; }

        public string PurchaseToken { get; set; }
    }
}
