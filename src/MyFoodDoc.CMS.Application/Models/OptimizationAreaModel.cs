using MyFoodDoc.Application.EnumEntities;

namespace MyFoodDoc.CMS.Application.Models
{
    public class OptimizationAreaModel : BaseModel<int>
    {
        public string Key { get; set; }

        public string Name { get; set; }

        public string Text { get; set; }

        public int? ImageId { get; set; }

        public ImageModel Image { get; set; }

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

        public bool IsCustom { get; set; }

        public static OptimizationAreaModel FromEntity(OptimizationArea entity)
        {
            return entity == null
                ? null
                : new OptimizationAreaModel()
                {
                    Id = entity.Id,
                    Key = entity.Key,
                    Name = entity.Name,
                    Text = entity.Text,
                    Image = entity.Image == null ? null : ImageModel.FromEntity(entity.Image),
                    LineGraphUpperLimit = entity.LineGraphUpperLimit,
                    LineGraphLowerLimit = entity.LineGraphLowerLimit,
                    LineGraphOptimal = entity.LineGraphOptimal,
                    OptimalLineGraphTitle = entity.OptimalLineGraphTitle,
                    OptimalLineGraphText = entity.OptimalLineGraphText,
                    BelowOptimalLineGraphTitle = entity.BelowOptimalLineGraphTitle,
                    BelowOptimalLineGraphText = entity.BelowOptimalLineGraphText,
                    AboveOptimalLineGraphTitle = entity.AboveOptimalLineGraphTitle,
                    AboveOptimalLineGraphText = entity.AboveOptimalLineGraphText,
                    OptimalPieChartTitle = entity.OptimalPieChartTitle,
                    OptimalPieChartText = entity.OptimalPieChartText,
                    BelowOptimalPieChartTitle = entity.BelowOptimalPieChartTitle,
                    BelowOptimalPieChartText = entity.BelowOptimalPieChartText,
                    AboveOptimalPieChartTitle = entity.AboveOptimalPieChartTitle,
                    AboveOptimalPieChartText = entity.AboveOptimalPieChartText,
                    IsCustom = entity.Type == MyFoodDoc.Application.Enums.OptimizationAreaType.Сustom
                };
        }

        public OptimizationArea ToEntity()
        {
            return new OptimizationArea()
            {
                Id = this.Id,
                Key = this.Key,
                Name = this.Name,
                Text = this.Text,
                ImageId = this.Image.Id,
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
