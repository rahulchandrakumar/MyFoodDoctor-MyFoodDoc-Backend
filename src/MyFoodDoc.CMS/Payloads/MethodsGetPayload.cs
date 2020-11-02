using System.Text.Json.Serialization;
using MyFoodDoc.CMS.Payloads.Base;

namespace MyFoodDoc.CMS.Payloads
{
    public class MethodsGetPayload : BasePaginatedPayload<object>
    {
        [JsonPropertyName("parentId")]
        public int? ParentId { get; set; }
    }
}
