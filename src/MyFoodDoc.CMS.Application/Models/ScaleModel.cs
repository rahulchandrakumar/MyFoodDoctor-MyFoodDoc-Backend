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

        public string TypeCode { get; set; }

        public string TypeTitle { get; set; }

        public string TypeText { get; set; }

        public string Characterization { get; set; }

        public string Reason { get; set; }

        public string Treatment { get; set; }

        public ImageModel Image { get; set; }

        public static ScaleModel FromEntity(Scale entity)
        {
            return entity == null ? null : new ScaleModel()
            {
                Id = entity.Id,
                Title = entity.Title,
                Text = entity.Text,
                Order = entity.Order,
                TypeCode = entity.TypeCode,
                TypeTitle = entity.TypeTitle,
                TypeText = entity.TypeText,
                Characterization = entity.Characterization,
                Reason = entity.Reason,
                Treatment = entity.Treatment,
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
                TypeCode = this.TypeCode,
                TypeTitle = this.TypeTitle,
                TypeText = this.TypeText,
                Characterization = this.Characterization,
                Reason = this.Reason,
                Treatment = this.Treatment,
                ImageId = this.Image.Id
            };
        }
    }
}
