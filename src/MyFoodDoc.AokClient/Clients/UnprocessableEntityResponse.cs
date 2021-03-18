using System.Text.Json.Serialization;

namespace MyFoodDoc.AokClient.Clients
{
    public class UnprocessableEntityResponse
    {
        [JsonPropertyName("errors")]
        public string[] Errors { get; set; }
    }
}
