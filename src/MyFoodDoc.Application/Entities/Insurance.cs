using MyFoodDoc.Application.Abstractions;

namespace MyFoodDoc.Application.Entities
{
    public class Insurance : AbstractAuditableEntity
    {
        public string Name { get; set; }
    }
}
