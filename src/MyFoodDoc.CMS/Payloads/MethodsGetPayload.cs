using MyFoodDoc.CMS.Payloads.Base;
using System.Text.Json.Serialization;

namespace MyFoodDoc.CMS.Payloads
{
    public class MethodsGetPayload : BasePaginatedPayload<object>
    {
        [JsonPropertyName("parentId")]
        public int? ParentId { get; set; }
    }
}
