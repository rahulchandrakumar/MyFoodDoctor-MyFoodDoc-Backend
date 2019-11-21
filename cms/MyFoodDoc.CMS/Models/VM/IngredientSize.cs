using MyFoodDoc.CMS.Models.VMBase;

namespace MyFoodDoc.CMS.Models.VM
{
    public class IngredientSize : ColabDataTableBaseModel
    {
        public string Name { get; set; }
        public decimal Amount { get; set; }
    }
}
