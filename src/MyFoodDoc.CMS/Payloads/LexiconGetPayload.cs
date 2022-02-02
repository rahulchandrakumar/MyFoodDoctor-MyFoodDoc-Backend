using MyFoodDoc.CMS.Payloads.Base;
using System.Text.Json.Serialization;

namespace MyFoodDoc.CMS.Payloads
{
    public class LexiconGetPayload : BasePaginatedPayload<object>
    {
        [JsonPropertyName("categoryId")]
        public int CategoryId { get; set; }
    }
}
