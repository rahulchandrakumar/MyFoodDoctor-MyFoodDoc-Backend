using MyFoodDoc.CMS.Application.Models;
using System.Text.Json.Serialization;

namespace MyFoodDoc.CMS.Models
{
    public class Image
    {
        [JsonPropertyName("Id")]
        public int Id { get; set; }
        [JsonPropertyName("Url")]
        public string Url { get; set; }

        public static Image FromModel(ImageModel model)
        {
            return new Image()
            {
                Id = model.Id,
                Url = model.Url
            };
        }

        public ImageModel ToModel()
        {
            return new ImageModel()
            {
                Id = this.Id,
                Url = this.Url
            };
        }
    }
}
