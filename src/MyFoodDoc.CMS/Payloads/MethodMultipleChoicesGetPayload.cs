using MyFoodDoc.CMS.Payloads.Base;
using System.Text.Json.Serialization;

namespace MyFoodDoc.CMS.Payloads
{
    public class MethodMultipleChoicesGetPayload : BasePaginatedPayload<object>
    {
        [JsonPropertyName("methodId")]
        public int MethodId { get; set; }
    }
}
