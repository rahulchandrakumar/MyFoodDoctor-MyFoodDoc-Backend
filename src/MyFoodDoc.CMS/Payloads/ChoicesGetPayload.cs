using System.Text.Json.Serialization;
using MyFoodDoc.CMS.Payloads.Base;

namespace MyFoodDoc.CMS.Payloads
{
    public class ChoicesGetPayload : BasePaginatedPayload<object>
    {
        [JsonPropertyName("questionId")]
        public int QuestionId { get; set; }
    }
}
