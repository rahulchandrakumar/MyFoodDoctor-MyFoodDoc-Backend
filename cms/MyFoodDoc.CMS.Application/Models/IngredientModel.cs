using MyFoodDoc.Application.Entites;

namespace MyFoodDoc.CMS.Application.Models
{
    public class IngredientModel : BaseModel<int>
    {
        public string BlsId { get; set; }
        public string Name { get; set; }
        public int? Amount { get; set; }

        public static IngredientModel FromEntity(Ingredient entity)
        {
            return new IngredientModel()
            {
                Id = entity.Id,
                Amount = entity.Amount,
                BlsId = entity.ExternalKey,
                Name = entity.Name
            };
        }

        public Ingredient ToEntity()
        {
            return new Ingredient()
            {
                Id = this.Id,
                Amount = this.Amount,
                ExternalKey = this.BlsId,
                Name = this.Name
            };
        }
    }
}
