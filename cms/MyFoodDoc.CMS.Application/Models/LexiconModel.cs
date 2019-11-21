using MyFoodDoc.Application.Entites;
using System;

namespace MyFoodDoc.CMS.Application.Models
{
    public class LexiconModel: BaseModel<int>
    {
        public string TitleLong { get; set; }
        public string TitleShort { get; set; }
        public string Text { get; set; }
        public ImageModel Image { get; set; }
        public int ImageId { get; set; }

        public static LexiconModel FromEntity(LexiconEntry entity)
        {
            return new LexiconModel()
            {
                Text = entity.Text,
                Id = entity.Id,
                Image = ImageModel.FromEntity(entity.Image),
                TitleLong = entity.TitleLong,
                TitleShort = entity.TitleShort
            };
        }

        public LexiconEntry ToEntity()
        {
            return new LexiconEntry()
            {
                Id = this.Id,
                ImageId = this.Image.Id,
                Text = this.Text,
                TitleLong = this.TitleLong,
                TitleShort = this.TitleShort
            };
        }
    }
}
