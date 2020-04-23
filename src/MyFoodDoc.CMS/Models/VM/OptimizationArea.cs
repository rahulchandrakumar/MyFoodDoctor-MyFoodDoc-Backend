using MyFoodDoc.CMS.Application.Models;
using MyFoodDoc.CMS.Models.VMBase;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyFoodDoc.CMS.Models.VM
{
    public class OptimizationArea : VMBase.BaseModel<int>
    {
        public string Key { get; set; }

        public string Name { get; set; }

        public string Text { get; set; }

        public Image Image { get; set; }

        public static OptimizationArea FromModel(OptimizationAreaModel model)
        {
            return model == null ? null : new OptimizationArea()
            {
                Id = model.Id,
                Key = model.Key,
                Name = model.Name,
                Text = model.Text,
                Image = Image.FromModel(model.Image)
            };
        }

        public OptimizationAreaModel ToModel()
        {
            return new OptimizationAreaModel()
            {
                Id = this.Id,
                Key = this.Key,
                Name = this.Name,
                Text = this.Text,
                Image = this.Image.ToModel()
            };
        }
    }
}
