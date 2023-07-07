using System.Collections.Generic;

namespace MyFoodDoc.App.Application.Models.StatisticsDto;

public record UserFavouriteDto(int FavouriteId, IEnumerable<FavouriteMealIngredientDto> Ingredient);