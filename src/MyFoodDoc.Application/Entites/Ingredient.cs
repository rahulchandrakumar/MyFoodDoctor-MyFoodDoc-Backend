using MyFoodDoc.Application.Abstractions;

namespace MyFoodDoc.Application.Entites
{
    public class Ingredient : AbstractAuditEntity<int>
    {
        public string Name { get; set; }
    }
}
