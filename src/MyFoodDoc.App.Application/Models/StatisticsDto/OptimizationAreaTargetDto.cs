using MyFoodDoc.Application.Enums;

namespace MyFoodDoc.App.Application.Models.StatisticsDto;

public record OptimizationAreaTargetDto(
    OptimizationAreaType Type,
    int? ImageId,
    string Key,
    string Name,
    string Text,
    decimal? LineGraphUpperLimit,
    decimal? LineGraphLowerLimit,
    decimal? LineGraphOptimal,
    string AboveOptimalLineGraphTitle,
    string AboveOptimalLineGraphText,
    string BelowOptimalLineGraphTitle,
    string BelowOptimalLineGraphText,
    string OptimalLineGraphTitle,
    string OptimalLineGraphText,
    string AboveOptimalPieChartTitle,
    string AboveOptimalPieChartText,
    string BelowOptimalPieChartTitle,
    string BelowOptimalPieChartText,
    string OptimalPieChartTitle,
    string OptimalPieChartText
);