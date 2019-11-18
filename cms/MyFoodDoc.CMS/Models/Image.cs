using MyFoodDoc.CMS.Application.Models;

namespace MyFoodDoc.CMS.Models
{
    public class Image
    {
        public int Id { get; set; }
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
