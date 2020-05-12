using System;
using System.Collections.Generic;
using System.Text;
using MyFoodDoc.Application.Entites.Courses;

namespace MyFoodDoc.CMS.Application.Models
{
    public class SubchapterModel : BaseModel<int>
    {
        public string Title { get; set; }

        public string Text { get; set; }

        public int Order { get; set; }

        public int ChapterId { get; set; }

        public static SubchapterModel FromEntity(Subchapter entity)
        {
            return entity == null ? null : new SubchapterModel()
            {
                Id = entity.Id,
                Title = entity.Title,
                Text = entity.Text,
                Order = entity.Order,
                ChapterId = entity.ChapterId
            };
        }

        public Subchapter ToEntity()
        {
            return new Subchapter()
            {
                Id = this.Id,
                Title = this.Title,
                Text = this.Text,
                Order = this.Order,
                ChapterId = this.ChapterId
            };
        }
    }
}
