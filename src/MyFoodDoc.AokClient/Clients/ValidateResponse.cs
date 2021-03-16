using System.Text.Json.Serialization;

namespace MyFoodDoc.AokClient.Clients
{
    public class ValidateResponse
    {
        [JsonPropertyName("registration_token")]
        public string RegistrationToken { get; set; }
    }
}
