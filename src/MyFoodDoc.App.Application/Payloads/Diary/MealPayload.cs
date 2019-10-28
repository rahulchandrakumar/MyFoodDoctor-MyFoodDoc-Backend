using MyFoodDoc.Application.Enums;
using System.Collections.Generic;

namespace MyFoodDoc.App.Application.Payloads.Diary
{
    public class MealPayload
    {
        public string Time { get; set; }

        public MealType Type { get; set; }

        public ICollection<MealIngredient> Ingredients { get; set; }

        public int Mood { get; set; }

    }
}
