﻿using MyFoodDoc.Application.Enums;
using System;
using System.Collections.Generic;

namespace MyFoodDoc.App.Application.Payloads.Diary
{
    public class UpdateMealPayload
    {
        public TimeSpan Time { get; set; }

        public MealType Type { get; set; }

        public IEnumerable<IngredientPayload> Ingredients { get; set; }

        public IEnumerable<MealFavouritePayload> Favourites { get; set; }

        public int? Mood { get; set; }
    }
}
