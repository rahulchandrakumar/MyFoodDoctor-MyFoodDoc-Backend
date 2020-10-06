using MyFoodDoc.CMS.Application.Models;
using MyFoodDoc.CMS.Models.VMBase;

namespace MyFoodDoc.CMS.Models.VM
{
    public class LexiconItem : VMBase.BaseModel<int>
    {
        public string TitleLong { get; set; }
        public string TitleShort { get; set; }
        public string Text { get; set; }
        public Image Image { get; set; }
        public int CategoryId { get; set; }

        public static LexiconItem FromModel(LexiconModel model)
        {
            return model == null ? null : new LexiconItem()
            {
                Id = model.Id,
                TitleShort = model.TitleShort,
                TitleLong = model.TitleLong,
                Text = model.Text,
                Image = Image.FromModel(model.Image),
                CategoryId = model.CategoryId
            };
        }

        public LexiconModel ToModel()
        {
            return new LexiconModel()
            {
                Id = this.Id,
                TitleShort = this.TitleShort,
                TitleLong = this.TitleLong,
                Text = this.Text,
                Image = this.Image.ToModel(),
                CategoryId = this.CategoryId
            };
        }
    }
}
