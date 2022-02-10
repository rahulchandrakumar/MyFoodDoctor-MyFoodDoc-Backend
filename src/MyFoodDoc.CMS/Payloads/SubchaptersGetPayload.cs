using MyFoodDoc.CMS.Payloads.Base;
using System.Text.Json.Serialization;

namespace MyFoodDoc.CMS.Payloads
{
    public class SubchaptersGetPayload : BasePaginatedPayload<object>
    {
        [JsonPropertyName("chapterId")]
        public int ChapterId { get; set; }
    }
}
