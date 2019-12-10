using MyFoodDoc.Application.Abstractions;

namespace MyFoodDoc.Application.Entites
{
    public class Ingredient : AbstractAuditableEntity
    {
        public string Name { get; set; }

        public string ExternalKey { get; set; }

        public int? Amount { get; set; }
    }
}
