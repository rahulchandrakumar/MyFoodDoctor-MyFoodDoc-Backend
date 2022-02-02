using MyFoodDoc.Application.Enums;
using MyFoodDoc.AppStoreClient.Clients;
using System.Threading.Tasks;

namespace MyFoodDoc.AppStoreClient.Abstractions
{
    public interface IAppStoreClient
    {
        Task<ReceiptValidationResult> ValidateReceipt(SubscriptionType subscriptionType, string receiptData);
    }
}
