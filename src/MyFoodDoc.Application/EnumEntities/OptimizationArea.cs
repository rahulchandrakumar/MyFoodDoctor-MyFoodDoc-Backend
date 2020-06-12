using MyFoodDoc.Application.Abstractions;
using MyFoodDoc.Application.Entites;

namespace MyFoodDoc.Application.EnumEntities
{
    public class OptimizationArea : AbstractEnumEntity
    {
        public string Text { get; set; }

        public int? ImageId { get; set; }

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
    }
}
