using MyFoodDoc.CMS.Application.Models;

namespace MyFoodDoc.CMS.Models.VM
{
    public class Indication : BaseModel<int>
    {
        public string Key { get; set; }

        public string Name { get; set; }

        public static Indication FromModel(IndicationModel model)
        {
            return model == null ? null : new Indication()
            {
                Id = model.Id,
                Key = model.Key,
                Name = model.Name
            };
        }
    }
}
