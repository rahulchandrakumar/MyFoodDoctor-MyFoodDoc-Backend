using MyFoodDoc.Application.Entities;

namespace MyFoodDoc.CMS.Application.Models
{
    public class LexiconModel : BaseModel<int>
    {
        public string TitleLong { get; set; }

        public string TitleShort { get; set; }

        public string Text { get; set; }

        public ImageModel Image { get; set; }

        public int CategoryId { get; set; }

        public static LexiconModel FromEntity(LexiconEntry entity)
        {
            return entity == null ? null : new LexiconModel()
            {
                Id = entity.Id,
                TitleShort = entity.TitleShort,
                TitleLong = entity.TitleLong,
                Text = entity.Text,
                Image = ImageModel.FromEntity(entity.Image),
                CategoryId = entity.CategoryId
            };
        }

        public LexiconEntry ToEntity()
        {
            return new LexiconEntry()
            {
                Id = this.Id,
                TitleShort = this.TitleShort,
                TitleLong = this.TitleLong,
                Text = this.Text,
                ImageId = this.Image.Id,
                CategoryId = this.CategoryId
            };
        }
    }
}
