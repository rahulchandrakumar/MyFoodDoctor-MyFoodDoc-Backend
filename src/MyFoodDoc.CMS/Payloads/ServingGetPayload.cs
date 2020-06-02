using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace MyFoodDoc.CMS.Payloads
{
    public class ServingGetPayload
    {
        [JsonPropertyName("foodId")]
        public long FoodId { get; set; }

        [JsonPropertyName("servingId")]
        public long ServingId { get; set; }
    }
}
