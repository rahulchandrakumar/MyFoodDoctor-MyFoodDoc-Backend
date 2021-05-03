using System.Threading.Tasks;
using MyFoodDoc.Application.Enums;
using MyFoodDoc.GooglePlayStoreClient.Clients;

namespace MyFoodDoc.GooglePlayStoreClient.Abstractions
{
    public interface IGooglePlayStoreClient
    {
        Task<PurchaseValidationResult> ValidatePurchase(SubscriptionType subscriptionType, string subscriptionId, string purchaseToken);
    }
}
