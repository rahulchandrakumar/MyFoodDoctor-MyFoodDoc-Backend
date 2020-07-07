using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MyFoodDoc.CMS.Application.Models;

namespace MyFoodDoc.CMS.Models.VM
{
    public class Subchapter : VMBase.BaseModel<int>
    {
        public string Title { get; set; }

        public string Text { get; set; }

        public int Order { get; set; }

        public int ChapterId { get; set; }

        public static Subchapter FromModel(SubchapterModel model)
        {
            return model == null ? null : new Subchapter()
            {
                Id = model.Id,
                Title = model.Title,
                Text = model.Text,
                Order = model.Order,
                ChapterId = model.ChapterId
            };
        }

        public SubchapterModel ToModel()
        {
            return new SubchapterModel()
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
