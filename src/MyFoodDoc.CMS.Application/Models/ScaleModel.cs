using System;
using System.Collections.Generic;
using System.Text;
using MyFoodDoc.Application.Entities.Psychogramm;

namespace MyFoodDoc.CMS.Application.Models
{
    public class ScaleModel : BaseModel<int>
    {
        public string Title { get; set; }

        public string Text { get; set; }

        public int Order { get; set; }

        public ImageModel Image { get; set; }

        public static ScaleModel FromEntity(Scale entity)
        {
            return entity == null ? null : new ScaleModel()
            {
                Id = entity.Id,
                Title = entity.Title,
                Text = entity.Text,
                Order = entity.Order,
                Image = ImageModel.FromEntity(entity.Image),
            };
        }

        public Scale ToEntity()
        {
            return new Scale()
            {
                Id = this.Id,
                Title = this.Title,
                Text = this.Text,
                Order = this.Order,
                ImageId = this.Image.Id
            };
        }
    }
}
