using System.Threading.Tasks;
using MyFoodDoc.GooglePlayStoreClient.Clients;

namespace MyFoodDoc.GooglePlayStoreClient.Abstractions
{
    public interface IGooglePlayStoreClient
    {
        Task<PurchaseValidationResult> ValidatePurchase(string subscriptionId, string purchaseToken);
    }
}
