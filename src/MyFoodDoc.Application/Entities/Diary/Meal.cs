using MyFoodDoc.Application.Entities.Abstractions;
using MyFoodDoc.Application.Enums;
using System;
using System.Collections.Generic;

namespace MyFoodDoc.Application.Entities.Diary
{
    public class Meal : AbstractDiaryEntity<string>
    {
        public TimeSpan Time { get; set; }

        public MealType Type { get; set; }

        public int? Mood { get; set; }

        public ICollection<MealIngredient> Ingredients { get; set; }

        public ICollection<MealFavourite> Favourites { get; set; }
    }
}
