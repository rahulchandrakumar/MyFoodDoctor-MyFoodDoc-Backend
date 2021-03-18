using System;
using System.Text.Json.Serialization;

namespace MyFoodDoc.App.Application.Payloads.Aok
{
    public class AuthenticatePayload
    {
        [JsonPropertyName("insurance_number")]
        public string InsuranceNumber { get; set; }

        [JsonPropertyName("date_of_birth")]
        public DateTime Birthday { get; set; }
    }
}
