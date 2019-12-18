using MyFoodDoc.CMS.Application.Models;

namespace MyFoodDoc.CMS.Models.VM
{
    public class Insurance : BaseModel<int>
    {
        public string Name { get; set; }

        public static Insurance FromModel(InsuranceModel model)
        {
            return model == null ? null : new Insurance()
            {
                Id = model.Id,
                Name = model.Name
            };
        }
    }
}
