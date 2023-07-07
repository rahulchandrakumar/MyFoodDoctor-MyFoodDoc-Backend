using System;
using System.Collections.Generic;
using MyFoodDoc.App.Application.Models.StatisticsDto;

namespace MyFoodDoc.App.Application.Abstractions.V2;

public interface IDiaryServiceV2
{
    bool IsDiaryFull(IEnumerable<MealDto> meals, DateTime userCreatedAt, DateTime onDate);
    bool IsZPPForbidden(double? userHeight, decimal? weight, bool eatingDisorder);
    decimal GetCorrectedWeight(decimal height, decimal weight);
}