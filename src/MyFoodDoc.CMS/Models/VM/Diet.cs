using MyFoodDoc.CMS.Application.Models;

namespace MyFoodDoc.CMS.Models.VM
{
    public class Diet : BaseModel<int>
    {
        public string Key { get; set; }

        public string Name { get; set; }

        public static Diet FromModel(DietModel model)
        {
            return model == null ? null : new Diet()
            {
                Id = model.Id,
                Key = model.Key,
                Name = model.Name
            };
        }
    }
}
