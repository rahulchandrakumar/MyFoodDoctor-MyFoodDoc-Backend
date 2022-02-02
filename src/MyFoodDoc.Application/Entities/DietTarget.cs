using MyFoodDoc.Application.Entities.Targets;
using MyFoodDoc.Application.EnumEntities;

namespace MyFoodDoc.Application.Entities
{
    public class DietTarget
    {
        public int DietId { get; set; }

        public int TargetId { get; set; }

        public Diet Diet { get; set; }

        public Target Target { get; set; }
    }
}
