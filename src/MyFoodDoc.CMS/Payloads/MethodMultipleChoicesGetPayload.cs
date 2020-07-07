using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using MyFoodDoc.CMS.Payloads.Base;

namespace MyFoodDoc.CMS.Payloads
{
    public class MethodMultipleChoicesGetPayload : BasePaginatedPayload<object>
    {
        [JsonPropertyName("methodId")]
        public int MethodId { get; set; }
    }
}
