using MyFoodDoc.Application.Entities.Targets;
using MyFoodDoc.Application.EnumEntities;

namespace MyFoodDoc.Application.Entities
{
    public class IndicationTarget
    {
        public int IndicationId { get; set; }

        public int TargetId { get; set; }

        public Indication Indication { get; set; }

        public Target Target { get; set; }
    }
}
