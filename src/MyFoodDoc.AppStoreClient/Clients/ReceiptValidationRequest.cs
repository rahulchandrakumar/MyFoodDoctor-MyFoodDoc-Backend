using Newtonsoft.Json;

namespace MyFoodDoc.AppStoreClient.Clients
{
    public class ReceiptValidationRequest
    {
        [JsonProperty("password")]
        public string Password { get; set; }

        [JsonProperty("exclude-old-transactions")]
        public bool ExcludeOldTransactions { get; set; }

        [JsonProperty("receipt-data")]
        public string ReceiptData { get; set; }
    }
}
