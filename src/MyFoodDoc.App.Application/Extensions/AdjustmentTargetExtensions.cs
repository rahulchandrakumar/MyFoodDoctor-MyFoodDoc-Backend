using MyFoodDoc.App.Application.Models.StatisticsDto;
using MyFoodDoc.Application.Entities.Targets;

namespace MyFoodDoc.App.Application.Extensions;

public static class AdjustmentTargetExtensions
{
    public static AdjustmentTargetDto ToAdjustmentTargetDto(this AdjustmentTarget adjustmentTarget)
    {
        return new AdjustmentTargetDto(
            adjustmentTarget.TargetId,
            adjustmentTarget.StepDirection,
            adjustmentTarget.Step,
            adjustmentTarget.TargetValue,
            adjustmentTarget.RecommendedText,
            adjustmentTarget.TargetText,
            adjustmentTarget.RemainText
        );
    }
}