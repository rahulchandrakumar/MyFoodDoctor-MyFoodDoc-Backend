using MyFoodDoc.CMS.Application.Models;

namespace MyFoodDoc.CMS.Models.VM
{
    public class Ingredient : VMBase.BaseModel<int>
    {
        public string BlsId { get; set; }
        public string Name { get; set; }
        public int? Amount { get; set; }

        public static Ingredient FromModel(IngredientModel model)
        {
            return model == null ? null : new Ingredient()
            {
                Id = model.Id,
                Amount = model.Amount,
                BlsId = model.BlsId,
                Name = model.Name
            };
        }

        public IngredientModel ToModel()
        {
            return new IngredientModel()
            {
                Id = this.Id,
                Amount = this.Amount,
                BlsId = this.BlsId,
                Name = this.Name
            };
        }
    }
}
