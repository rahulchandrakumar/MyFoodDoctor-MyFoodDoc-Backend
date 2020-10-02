using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using MyFoodDoc.AppStoreClient.Clients;

namespace MyFoodDoc.AppStoreClient.Abstractions
{
    public interface IAppStoreClient
    {
        Task<ReceiptValidationResult> ValidateReceipt(string receiptData);
    }
}
