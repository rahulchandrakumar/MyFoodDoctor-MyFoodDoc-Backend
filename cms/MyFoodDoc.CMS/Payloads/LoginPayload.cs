using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace MyFoodDoc.CMS.Payloads
{
    public class LoginPayload
    {
        [Required]
        [StringLength(100)]
        [JsonPropertyName("username")]
        public string Username { get; set; }

        [Required]
        [StringLength(100)]
        [JsonPropertyName("password")]
        public string Password { get; set; }

        [JsonPropertyName("rememberMe")]
        public bool RememberMe { get; set; }
    }
}
