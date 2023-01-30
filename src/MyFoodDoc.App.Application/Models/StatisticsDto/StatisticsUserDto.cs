using System;
using System.Collections.Generic;
using MyFoodDoc.Application.Enums;

namespace MyFoodDoc.App.Application.Models.StatisticsDto;

public class StatisticsUserDto 
{
    public DateTime Created { get; set; }

    public string Email { get; set; }

    public virtual bool IsAnamnesisCompleted =>
        Gender != null && Height != null && (Indications != null || Motivations != null);

    public int? Age { get; set; }

    public Gender? Gender { get; set; }

    public decimal? Height { get; set; }

    public int? InsuranceId { get; set; }

    public bool HasSubscription { get; set; }

    public bool HasZPPSubscription { get; set; }

    public bool EatingDisorder { get; set; }

    public ICollection<MotivationDto> Motivations { get; set; }

    public ICollection<IndicationDto> Indications { get; set; }

    public ICollection<DietDto> Diets { get; set; }

    public ICollection<UserWeightDto> Weights { get; set; }
    public ICollection<MealDto> Meals { get; set; }
    public ICollection<UserTargetDto> UserTargets { get; set; }
    public ICollection<UserFavouriteDto> FavouriteIngredientDtos { get; set; }
}