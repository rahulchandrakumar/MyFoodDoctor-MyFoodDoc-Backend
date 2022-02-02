using MyFoodDoc.Application.Enums;
using MyFoodDoc.GooglePlayStoreClient.Clients;
using System.Threading.Tasks;

namespace MyFoodDoc.GooglePlayStoreClient.Abstractions
{
    public interface IGooglePlayStoreClient
    {
        Task<PurchaseValidationResult> ValidatePurchase(SubscriptionType subscriptionType, string subscriptionId, string purchaseToken);
    }
}
