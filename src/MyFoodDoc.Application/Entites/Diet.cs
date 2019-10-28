using MyFoodDoc.Application.Abstractions;

namespace MyFoodDoc.Application.Entites
{
    public class Diet : AbstractAuditEntity<int>
    {
        public string Key { get; set; }

        public string Name { get; set; }
    }
}
