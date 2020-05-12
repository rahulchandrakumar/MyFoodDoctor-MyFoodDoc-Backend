using System.Text.Json.Serialization;
using MyFoodDoc.CMS.Payloads.Base;

namespace MyFoodDoc.CMS.Payloads
{
    public class ChaptersGetPayload : BasePaginatedPayload<object>
    {
        [JsonPropertyName("courseId")]
        public int CourseId { get; set; }
    }
}
