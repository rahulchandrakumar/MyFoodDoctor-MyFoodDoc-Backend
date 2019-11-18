using MyFoodDoc.Application.Abstractions;
using MyFoodDoc.Application.Enums;
using System.Collections.Generic;

namespace MyFoodDoc.Application.Entites
{
    public class Meal : AbstractAuditEntity<int>
    {
        public string Time { get; set; }

        public MealType Type { get; set; }

        public ICollection<MealIngredient> Ingredients { get; set; }

        public int Mood { get; set; }
    }
}
