using MyFoodDoc.CMS.Application.Models;
using MyFoodDoc.CMS.Models.VMBase;

namespace MyFoodDoc.CMS.Models.VM
{
    public class OptimizationArea : VMBase.BaseModel<int>
    {
        public string Key { get; set; }

        public string Name { get; set; }

        public string Text { get; set; }

        public Image Image { get; set; }

        public decimal? UpperLimit { get; set; }

        public decimal? LowerLimit { get; set; }

        public decimal? Optimal { get; set; }

        public static OptimizationArea FromModel(OptimizationAreaModel model)
        {
            return model == null ? null : new OptimizationArea()
            {
                Id = model.Id,
                Key = model.Key,
                Name = model.Name,
                Text = model.Text,
                Image = Image.FromModel(model.Image),
                UpperLimit = model.UpperLimit,
                LowerLimit = model.LowerLimit,
                Optimal = model.Optimal
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
                Image = this.Image.ToModel(),
                UpperLimit = this.UpperLimit,
                LowerLimit = this.LowerLimit,
                Optimal = this.Optimal
            };
        }
    }
}
