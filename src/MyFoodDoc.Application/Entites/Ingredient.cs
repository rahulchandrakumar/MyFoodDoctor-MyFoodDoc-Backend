using MyFoodDoc.Application.Abstractions;

namespace MyFoodDoc.Application.Entites
{
    public class Ingredient : AbstractAuditableEntity<int>
    {
        public string Name { get; set; }
    }
}
