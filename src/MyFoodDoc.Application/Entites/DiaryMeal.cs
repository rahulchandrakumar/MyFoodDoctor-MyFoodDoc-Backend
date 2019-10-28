using MyFoodDoc.Application.Abstractions;
using MyFoodDoc.Application.Enums;
using System.Collections.Generic;

namespace MyFoodDoc.Application.Entites
{
    public class DiaryMeal : AbstractAuditEntity<int>
    {
        public string Time { get; set; }

        public MealType Type { get; set; }

        public ICollection<DiaryMealIngredient> Ingredients { get; set; }

        public int Mood { get; set; }
    }
}
