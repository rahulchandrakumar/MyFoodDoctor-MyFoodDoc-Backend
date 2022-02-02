using MyFoodDoc.CMS.Payloads.Base;
using System.Text.Json.Serialization;


namespace MyFoodDoc.CMS.Payloads
{
    public class TargetsGetPayload : BasePaginatedPayload<object>
    {
        [JsonPropertyName("optimizationAreaId")]
        public int OptimizationAreaId { get; set; }
    }
}
