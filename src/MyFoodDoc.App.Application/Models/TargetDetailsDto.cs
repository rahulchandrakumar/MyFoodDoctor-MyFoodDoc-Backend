using MyFoodDoc.App.Application.Models.StatisticsDto;
using MyFoodDoc.Application.EnumEntities;
using MyFoodDoc.Application.Enums;

namespace MyFoodDoc.App.Application.Models
{
    public class TargetDetailsDto
    {
        public int Id { get; set; }
        public int OptimizationAreaId { get; set; }
        public TriggerOperator TriggerOperator { get; set; }
        public decimal TriggerValue { get; set; }
        public int Threshold { get; set; }
        public TargetPriority Priority { get; set; }
        public string Title { get; set; }
        public string Text { get; set; }
        public TargetType Type { get; set; }
        public int ImageId { get; set; }
        public OptimizationAreaTargetDto OptimizationArea { get; set; }
    }
}
