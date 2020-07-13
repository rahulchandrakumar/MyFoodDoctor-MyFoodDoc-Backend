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

        public decimal? LineGraphUpperLimit { get; set; }

        public decimal? LineGraphLowerLimit { get; set; }

        public decimal? LineGraphOptimal { get; set; }

        public string OptimalLineGraphTitle { get; set; }

        public string OptimalLineGraphText { get; set; }

        public string BelowOptimalLineGraphTitle { get; set; }

        public string BelowOptimalLineGraphText { get; set; }

        public string AboveOptimalLineGraphTitle { get; set; }

        public string AboveOptimalLineGraphText { get; set; }

        public string OptimalPieChartTitle { get; set; }

        public string OptimalPieChartText { get; set; }

        public string BelowOptimalPieChartTitle { get; set; }

        public string BelowOptimalPieChartText { get; set; }

        public string AboveOptimalPieChartTitle { get; set; }

        public string AboveOptimalPieChartText { get; set; }

        public static OptimizationArea FromModel(OptimizationAreaModel model)
        {
            return model == null ? null : new OptimizationArea()
            {
                Id = model.Id,
                Key = model.Key,
                Name = model.Name,
                Text = model.Text,
                Image = model.Image == null ? null : Image.FromModel(model.Image),
                LineGraphUpperLimit = model.LineGraphUpperLimit,
                LineGraphLowerLimit = model.LineGraphLowerLimit,
                LineGraphOptimal = model.LineGraphOptimal,
                OptimalLineGraphTitle = model.OptimalLineGraphTitle,
                OptimalLineGraphText = model.OptimalLineGraphText,
                BelowOptimalLineGraphTitle = model.BelowOptimalLineGraphTitle,
                BelowOptimalLineGraphText = model.BelowOptimalLineGraphText,
                AboveOptimalLineGraphTitle = model.AboveOptimalLineGraphTitle,
                AboveOptimalLineGraphText = model.AboveOptimalLineGraphText,
                OptimalPieChartTitle = model.OptimalPieChartTitle,
                OptimalPieChartText = model.OptimalPieChartText,
                BelowOptimalPieChartTitle = model.BelowOptimalPieChartTitle,
                BelowOptimalPieChartText = model.BelowOptimalPieChartText,
                AboveOptimalPieChartTitle = model.AboveOptimalPieChartTitle,
                AboveOptimalPieChartText = model.AboveOptimalPieChartText
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
                LineGraphUpperLimit = this.LineGraphUpperLimit,
                LineGraphLowerLimit = this.LineGraphLowerLimit,
                LineGraphOptimal = this.LineGraphOptimal,
                OptimalLineGraphTitle = this.OptimalLineGraphTitle,
                OptimalLineGraphText = this.OptimalLineGraphText,
                BelowOptimalLineGraphTitle = this.BelowOptimalLineGraphTitle,
                BelowOptimalLineGraphText = this.BelowOptimalLineGraphText,
                AboveOptimalLineGraphTitle = this.AboveOptimalLineGraphTitle,
                AboveOptimalLineGraphText = this.AboveOptimalLineGraphText,
                OptimalPieChartTitle = this.OptimalPieChartTitle,
                OptimalPieChartText = this.OptimalPieChartText,
                BelowOptimalPieChartTitle = this.BelowOptimalPieChartTitle,
                BelowOptimalPieChartText = this.BelowOptimalPieChartText,
                AboveOptimalPieChartTitle = this.AboveOptimalPieChartTitle,
                AboveOptimalPieChartText = this.AboveOptimalPieChartText
            };
        }
    }
}
