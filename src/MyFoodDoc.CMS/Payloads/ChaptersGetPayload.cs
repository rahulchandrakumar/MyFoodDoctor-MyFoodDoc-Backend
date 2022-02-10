using MyFoodDoc.CMS.Payloads.Base;
using System.Text.Json.Serialization;

namespace MyFoodDoc.CMS.Payloads
{
    public class ChaptersGetPayload : BasePaginatedPayload<object>
    {
        [JsonPropertyName("courseId")]
        public int CourseId { get; set; }
    }
}
