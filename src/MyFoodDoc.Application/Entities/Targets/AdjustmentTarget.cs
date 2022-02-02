using MyFoodDoc.Application.Abstractions;
using MyFoodDoc.Application.Enums;

namespace MyFoodDoc.Application.Entities.Targets
{
    public class AdjustmentTarget : AbstractAuditableEntity
    {
        public int TargetId { get; set; }

        public int TargetValue { get; set; }

        public int Step { get; set; }

        public AdjustmentTargetStepDirection StepDirection { get; set; }

        public string RecommendedText { get; set; }

        public string TargetText { get; set; }

        public string RemainText { get; set; }

        public Target Target { get; set; }
    }
}
