using MyFoodDoc.Application.Enums;
using System;
using System.Collections.Generic;

namespace MyFoodDoc.App.Application.Payloads.Diary
{
    public class UpdateMealPayload
    {
        public TimeSpan Time { get; set; }

        public MealType Type { get; set; }

        public IEnumerable<Ingredient> Ingredients { get; set; }

        public int? Mood { get; set; }

        public class Ingredient
        {
            public int Id { get; set; }

            public int Amount { get; set; }
        }
    }
}
