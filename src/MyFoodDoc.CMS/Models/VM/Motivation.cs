using MyFoodDoc.CMS.Application.Models;

namespace MyFoodDoc.CMS.Models.VM
{
    public class Motivation : BaseModel<int>
    {
        public string Key { get; set; }

        public string Name { get; set; }

        public static Motivation FromModel(MotivationModel model)
        {
            return model == null ? null : new Motivation()
            {
                Id = model.Id,
                Key = model.Key,
                Name = model.Name
            };
        }
    }
}
