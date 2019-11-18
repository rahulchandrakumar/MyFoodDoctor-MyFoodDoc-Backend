using MyFoodDoc.Application.Abstractions;

namespace MyFoodDoc.Application.Entites
{
    public class Insurance : AbstractAuditableEntity<int>
    {
        public string Name { get; set; }
    }
}
