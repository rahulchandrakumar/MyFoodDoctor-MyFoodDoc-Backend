using MyFoodDoc.CMS.Application.Models;
using MyFoodDoc.CMS.Models.VMBase;

namespace MyFoodDoc.CMS.Models.VM
{
    public class LexiconCategory : VMBase.BaseModel<int>
    {
        public string Title { get; set; }

        public Image Image { get; set; }

        public static LexiconCategory FromModel(LexiconCategoryModel model)
        {
            return model == null ? null : new LexiconCategory()
            {
                Id = model.Id,
                Title = model.Title,
                Image = Image.FromModel(model.Image)
            };
        }

        public LexiconCategoryModel ToModel()
        {
            return new LexiconCategoryModel()
            {
                Id = this.Id,
                Title = this.Title,
                Image = this.Image.ToModel()
            };
        }
    }
}
