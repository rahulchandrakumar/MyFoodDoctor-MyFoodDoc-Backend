namespace MyFoodDoc.App.Application.Models.StatisticsDto;

public record FavouriteMealIngredientDto(decimal? Protein,
    decimal ProteinExternal,
    bool ContainsPlantProtein,
    decimal? Calories,
    decimal CaloriesExternal,
    decimal? Sugar,
    decimal SugarExternal,
    decimal? Vegetables,
    decimal Amount);