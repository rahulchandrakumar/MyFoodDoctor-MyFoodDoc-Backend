using System.Text.Json.Serialization;
using MyFoodDoc.CMS.Payloads.Base;

namespace MyFoodDoc.CMS.Payloads
{
    public class SubchaptersGetPayload : BasePaginatedPayload<object>
    {
        [JsonPropertyName("chapterId")]
        public int ChapterId { get; set; }
    }
}
