using MyFoodDoc.CMS.Application.FilterModels;

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
    public class PortionsGetPayload
    {
        public int Take { get; set; }
        public int Skip { get; set; }
        public string Search { get; set; }
        public PortionsFilter Filter { get; set; }
    }
}
