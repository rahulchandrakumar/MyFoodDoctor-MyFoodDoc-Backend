using MyFoodDoc.Application.Entities.Targets;
using MyFoodDoc.Application.Enums;

namespace MyFoodDoc.Application.Entities
{
    public class ReportAdjustmentTarget : ReportValueTarget
    {
       // public override ValueTargetType Type { get; } = ValueTargetType.Adjustment;

        public new decimal? Value => TargetValue;

        public new AdjustmentTarget Target { get; protected set; }

        public decimal? TargetValue { get; set; }
    }
}
