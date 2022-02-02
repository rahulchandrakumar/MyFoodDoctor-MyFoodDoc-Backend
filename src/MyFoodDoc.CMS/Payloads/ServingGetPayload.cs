using System.Text.Json.Serialization;

namespace MyFoodDoc.CMS.Payloads
{
    public class ServingGetPayload
    {
        [JsonPropertyName("foodId")]
        public long FoodId { get; set; }

        [JsonPropertyName("servingId")]
        public long ServingId { get; set; }
    }
}
