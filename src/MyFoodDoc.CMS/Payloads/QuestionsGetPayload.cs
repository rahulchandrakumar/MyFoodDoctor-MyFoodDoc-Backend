using MyFoodDoc.CMS.Payloads.Base;
using System.Text.Json.Serialization;

namespace MyFoodDoc.CMS.Payloads
{
    public class QuestionsGetPayload : BasePaginatedPayload<object>
    {
        [JsonPropertyName("scaleId")]
        public int ScaleId { get; set; }
    }
}
