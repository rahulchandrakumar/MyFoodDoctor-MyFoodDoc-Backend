using MyFoodDoc.Application.Entites.Abstractions;
using MyFoodDoc.Application.EnumEntities;

namespace MyFoodDoc.Application.Entites
{
    public class DietMethod
    {
        public int DietId { get; set; }

        public int MethodId { get; set; }

        public Diet Diet { get; set; }

        public Method Method { get; set; }
    }
}
