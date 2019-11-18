namespace MyFoodDoc.CMS.Application.Models
{
    public class LexiconModel: BaseModel
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public ImageModel Image { get; set; }
    }
}
