using MyFoodDoc.Application.Abstractions;

namespace MyFoodDoc.Application.Entites
{
    public class Ingredient : AbstractAuditableEntity
    {
        public string Name { get; set; }
    }
}
