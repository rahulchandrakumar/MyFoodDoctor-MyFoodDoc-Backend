using MyFoodDoc.Application.Entites;

namespace MyFoodDoc.CMS.Application.Models
{
    public class InsuranceModel: BaseModel<int>
    {
        public string Name { get; set; }

        public static InsuranceModel FromEntity(Insurance entity)
        {
            return new InsuranceModel()
            {
                Id = entity.Id,
                Name = entity.Name
            };
        }
    }
}
