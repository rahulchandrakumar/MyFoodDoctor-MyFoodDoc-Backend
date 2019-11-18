using MyFoodDoc.Application.Abstractions;

namespace MyFoodDoc.Application.EnumEntities
{
    public class Indication : AbstractAuditEntity<int>
    {
        public string Key { get; set; }

        public string Name { get; set; }
    }
}
