using System;
using System.Collections.Generic;
using MyFoodDoc.Application.Enums;

namespace MyFoodDoc.App.Application.Models.StatisticsDto;

public record MealDto(
    DateTime Date,
    MealType Type,
    IEnumerable<MealIngredientDto> IngredientDtos,
    IEnumerable<int> MealFavouriteIds);