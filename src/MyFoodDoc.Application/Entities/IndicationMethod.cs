using MyFoodDoc.Application.Entities.Methods;
using MyFoodDoc.Application.EnumEntities;

namespace MyFoodDoc.Application.Entities
{
    public class IndicationMethod
    {
        public int IndicationId { get; set; }

        public int MethodId { get; set; }

        public bool IsContraindication { get; set; }

        public Indication Indication { get; set; }

        public Method Method { get; set; }
    }
}
