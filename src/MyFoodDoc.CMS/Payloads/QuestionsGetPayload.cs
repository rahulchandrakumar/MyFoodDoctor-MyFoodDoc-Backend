using System.Text.Json.Serialization;
using MyFoodDoc.CMS.Payloads.Base;

namespace MyFoodDoc.CMS.Payloads
{
    public class QuestionsGetPayload : BasePaginatedPayload<object>
    {
        [JsonPropertyName("scaleId")]
        public int ScaleId { get; set; }
    }
}
