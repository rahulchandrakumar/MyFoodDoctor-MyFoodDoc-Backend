using MyFoodDoc.Application.Entities.Methods;
using MyFoodDoc.Application.EnumEntities;

namespace MyFoodDoc.Application.Entities
{
    public class DietMethod
    {
        public int DietId { get; set; }

        public int MethodId { get; set; }

        public bool IsContraindication { get; set; }

        public Diet Diet { get; set; }

        public Method Method { get; set; }
    }
}
