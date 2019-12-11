using MyFoodDoc.CMS.Application.Models;
using MyFoodDoc.CMS.Models.VMBase;

namespace MyFoodDoc.CMS.Models.VM
{
    public class LexiconItem : ColabDataTableBaseModel<int>
    {
        public string TitleLong { get; set; }
        public string TitleShort { get; set; }
        public string Text { get; set; }
        public Image Image { get; set; }

        public static LexiconItem FromModel(LexiconModel model)
        {
            return new LexiconItem()
            {
                Id = model.Id,
                TitleLong = model.TitleLong,
                TitleShort = model.TitleShort,
                Text = model.Text,
                Image = Image.FromModel(model.Image)
            };
        }

        public LexiconModel ToModel()
        {
            return new LexiconModel()
            {
                Id = this.Id,
                TitleLong = this.TitleLong,
                TitleShort = this.TitleShort,
                Text = this.Text,
                Image = this.Image.ToModel()
            };
        }
    }
}
