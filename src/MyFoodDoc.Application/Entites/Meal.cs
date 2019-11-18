using MyFoodDoc.Application.Abstractions;
using MyFoodDoc.Application.Enums;
using System.Collections.Generic;

namespace MyFoodDoc.Application.Entites
{
    public class Meal : AbstractAuditableEntity<int>
    {
        public string Time { get; set; }

        public MealType Type { get; set; }

        public ICollection<MealIngredient> Ingredients { get; set; }

        public int Mood { get; set; }
    }
}
