using MyFoodDoc.App.Application.Models.StatisticsDto;
using MyFoodDoc.Application.EnumEntities;

namespace MyFoodDoc.App.Application.Extensions;

public static class OptimizationAreaTargetExtensions
{
    public static OptimizationAreaTargetDto ToOptimizationAreaTargetDto(this OptimizationArea optimizationArea)
    {
        return new OptimizationAreaTargetDto(
            optimizationArea.ImageId,
            optimizationArea.Key,
            optimizationArea.Name,
            optimizationArea.Text,
            optimizationArea.LineGraphUpperLimit,
            optimizationArea.LineGraphLowerLimit,
            optimizationArea.LineGraphOptimal,
            optimizationArea.AboveOptimalLineGraphTitle,
            optimizationArea.AboveOptimalLineGraphText,
            optimizationArea.BelowOptimalLineGraphTitle,
            optimizationArea.BelowOptimalLineGraphText,
            optimizationArea.OptimalLineGraphTitle,
            optimizationArea.OptimalLineGraphText,
            optimizationArea.AboveOptimalPieChartTitle,
            optimizationArea.AboveOptimalPieChartText,
            optimizationArea.BelowOptimalPieChartTitle,
            optimizationArea.BelowOptimalPieChartText,
            optimizationArea.OptimalPieChartTitle,
            optimizationArea.OptimalPieChartText
        );
    }
}