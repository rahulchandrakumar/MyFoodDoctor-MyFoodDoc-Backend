using MyFoodDoc.Application.Abstractions;
using System.Collections.Generic;

namespace MyFoodDoc.Application.Entites
{
    public class Ingredient : AbstractAuditableEntity
    {
        public string Name { get; set; }

        public string ExternalKey { get; set; }

        public int? Amount { get; set; }

        public ICollection<MealIngredient> Meals { get; set; }
    }
}
