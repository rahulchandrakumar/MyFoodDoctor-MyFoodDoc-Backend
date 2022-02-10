using MyFoodDoc.Application.Entities;

namespace MyFoodDoc.CMS.Application.Models
{
    public class LexiconCategoryModel : BaseModel<int>
    {
        public string Title { get; set; }

        public ImageModel Image { get; set; }

        public static LexiconCategoryModel FromEntity(LexiconCategory entity)
        {
            return entity == null ? null : new LexiconCategoryModel()
            {
                Id = entity.Id,
                Title = entity.Title,
                Image = ImageModel.FromEntity(entity.Image)
            };
        }

        public LexiconCategory ToEntity()
        {
            return new LexiconCategory()
            {
                Id = this.Id,
                Title = this.Title,
                ImageId = this.Image.Id
            };
        }
    }
}
