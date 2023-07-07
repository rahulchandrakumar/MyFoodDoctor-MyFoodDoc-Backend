using MyFoodDoc.Application.Enums;

namespace MyFoodDoc.App.Application.Models.StatisticsDto;

public record AdjustmentTargetDto(
    int TargetId,
    AdjustmentTargetStepDirection StepDirection,
    int Step,
    int TargetValue,
    string RecommendedText,
    string TargetText,
    string RemainText);