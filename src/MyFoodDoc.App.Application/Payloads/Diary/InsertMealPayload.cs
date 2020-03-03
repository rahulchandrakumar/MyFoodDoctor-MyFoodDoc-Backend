using MyFoodDoc.Application.Enums;
using System;
using System.Collections.Generic;

namespace MyFoodDoc.App.Application.Payloads.Diary
{
    public class InsertMealPayload
    {
        public DateTime Date { get; set; }

        public TimeSpan Time { get; set; }

        public MealType Type { get; set; }

        public IEnumerable<IngredientPayload> Ingredients { get; set; }

        public int? Mood { get; set; }
    }
}
