using System;
using MyFoodDoc.Application.Enums;

namespace MyFoodDoc.App.Application.Models.StatisticsDto;

public record OptimizationAreaTargetDto(
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
)
{
    public OptimizationAreaType Type => Enum.TryParse(Key, true, out OptimizationAreaType retVal) ? retVal : OptimizationAreaType.Сustom;
}