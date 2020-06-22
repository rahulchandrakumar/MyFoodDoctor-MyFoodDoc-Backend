using MyFoodDoc.Application.Entities.Abstractions;
using MyFoodDoc.Application.EnumEntities;

namespace MyFoodDoc.Application.Entities
{
    public class MotivationMethod
    {
        public int MotivationId { get; set; }

        public int MethodId { get; set; }

        public Motivation Motivation { get; set; }

        public Method Method { get; set; }
    }
}
