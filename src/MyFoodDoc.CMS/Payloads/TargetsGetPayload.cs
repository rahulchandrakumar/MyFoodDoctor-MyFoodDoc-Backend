using System.Text.Json.Serialization;
using MyFoodDoc.CMS.Payloads.Base;


namespace MyFoodDoc.CMS.Payloads
{
    public class TargetsGetPayload : BasePaginatedPayload<object>
    {
        [JsonPropertyName("optimizationAreaId")]
        public int OptimizationAreaId { get; set; }
    }
}
