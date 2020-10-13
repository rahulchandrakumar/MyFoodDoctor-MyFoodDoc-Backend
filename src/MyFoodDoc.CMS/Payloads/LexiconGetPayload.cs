using System.Text.Json.Serialization;
using MyFoodDoc.CMS.Payloads.Base;

namespace MyFoodDoc.CMS.Payloads
{
    public class LexiconGetPayload : BasePaginatedPayload<object>
    {
        [JsonPropertyName("categoryId")]
        public int CategoryId { get; set; }
    }
}
