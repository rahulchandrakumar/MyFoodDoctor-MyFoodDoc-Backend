using MyFoodDoc.CMS.Application.FilterModels;
using MyFoodDoc.CMS.Payloads.Base;

namespace MyFoodDoc.CMS.Payloads
{
    public class PortionsFilter
    {
        public byte State { get; set; }

        public IngredientFilter ToModel()
        {
            return new IngredientFilter()
            {
                State = (IngredientFilterState)State
            };
        }
    }
    public class PortionsGetPayload: BasePaginatedPayload<PortionsFilter>
    {
    }
}
