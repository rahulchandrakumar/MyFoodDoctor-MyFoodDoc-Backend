using MyFoodDoc.Application.Entites.Abstractions;
using MyFoodDoc.Application.EnumEntities;

namespace MyFoodDoc.Application.Entites
{
    public class MotivationMethod
    {
        public int MotivationId { get; set; }

        public int MethodId { get; set; }

        public Motivation Motivation { get; set; }

        public Method Method { get; set; }
    }
}
