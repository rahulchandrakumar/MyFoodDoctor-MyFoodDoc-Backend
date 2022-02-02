using MyFoodDoc.Application.Entities.Targets;
using MyFoodDoc.Application.EnumEntities;

namespace MyFoodDoc.Application.Entities
{
    public class MotivationTarget
    {
        public int MotivationId { get; set; }

        public int TargetId { get; set; }

        public Motivation Motivation { get; set; }

        public Target Target { get; set; }
    }
}
