using MyFoodDoc.Application.Abstractions;

namespace MyFoodDoc.Application.Entites
{
    public class Insurance : AbstractAuditableEntity
    {
        public string Name { get; set; }
    }
}
