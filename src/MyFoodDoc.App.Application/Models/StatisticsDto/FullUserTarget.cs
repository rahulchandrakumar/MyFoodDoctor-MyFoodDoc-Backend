using System.Collections.Generic;
using MyFoodDoc.Application.EnumEntities;
using MyFoodDoc.Application.Enums;

namespace MyFoodDoc.App.Application.Models.StatisticsDto;

public record FullUserTargetDto(int Id,
    OptimizationAreaTargetDto OptimizationArea,
    TriggerOperator TriggerOperator,
    decimal TriggerValue,
    int Threshold,
    TargetPriority Priority,
    string Title,
    string Text,
    TargetType Type,
    string ImageUrl,
    AdjustmentTargetDto AdjustmentTargetDto
);