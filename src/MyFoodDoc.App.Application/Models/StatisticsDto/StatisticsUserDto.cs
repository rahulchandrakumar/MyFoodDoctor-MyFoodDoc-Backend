using System;
using System.Collections.Generic;
using MyFoodDoc.Application.Enums;

namespace MyFoodDoc.App.Application.Models.StatisticsDto;

public class StatisticsUserDto
{
    public DateTime Created { get; set; }

    public string Email { get; set; }

    public virtual bool IsAnamnesisCompleted =>
        Gender != null && Height != null && (MotivationTargets != null || IndicationTargets != null);

    public int? Age { get; set; }

    public Gender? Gender { get; set; }

    public decimal? Height { get; set; }

    public int? InsuranceId { get; set; }

    public bool HasSubscription { get; set; }

    public bool HasZPPSubscription { get; set; }

    public bool EatingDisorder { get; set; }

    public IEnumerable<TargetDetailsDto> MotivationTargets { get; set; } = new List<TargetDetailsDto>();

    public IEnumerable<TargetDetailsDto> IndicationTargets { get; set; } = new List<TargetDetailsDto>();
    
    public ICollection<DietDto> Diets { get; set; } = new List<DietDto>();

    public ICollection<UserWeightDto> Weights { get; set; } = new List<UserWeightDto>();

    public ICollection<MealDto> Meals { get; set; } = new List<MealDto>();
    public ICollection<UserTargetDto> UserTargets { get; set; } = new List<UserTargetDto>();
    public UserFavouriteDto FavouriteIngredientDtos { get; set; }
}