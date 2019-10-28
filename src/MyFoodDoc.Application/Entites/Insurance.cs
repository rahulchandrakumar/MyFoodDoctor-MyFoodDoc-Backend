using MyFoodDoc.Application.Abstractions;

namespace MyFoodDoc.Application.Entites
{
    public class Insurance : AbstractAuditEntity<int>
    {
        public string Name { get; set; }
    }
}
