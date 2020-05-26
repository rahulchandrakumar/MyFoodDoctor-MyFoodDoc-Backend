using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using MyFoodDoc.CMS.Payloads.Base;

namespace MyFoodDoc.CMS.Payloads
{
    public class MethodsGetPayload : BasePaginatedPayload<object>
    {
        [JsonPropertyName("targetId")]
        public int TargetId { get; set; }
    }
}
