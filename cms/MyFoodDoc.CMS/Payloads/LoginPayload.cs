using System.Text.Json.Serialization;

namespace MyFoodDoc.CMS.Payloads
{
    public class LoginPayload
    {
        [JsonPropertyName("username")]
        public string Username { get; set; }

        [JsonPropertyName("password")]
        public string Password { get; set; }

        [JsonPropertyName("rememberMe")]
        public bool RememberMe { get; set; }
    }
}
