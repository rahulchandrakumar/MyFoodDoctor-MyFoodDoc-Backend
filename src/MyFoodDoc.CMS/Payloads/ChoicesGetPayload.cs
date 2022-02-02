using MyFoodDoc.CMS.Payloads.Base;
using System.Text.Json.Serialization;

namespace MyFoodDoc.CMS.Payloads
{
    public class ChoicesGetPayload : BasePaginatedPayload<object>
    {
        [JsonPropertyName("questionId")]
        public int QuestionId { get; set; }
    }
}
