using MyFoodDoc.Application.EnumEntities;

namespace MyFoodDoc.CMS.Application.Models
{
    public class IndicationModel : BaseModel<int>
    {
        public string Key { get; set; }

        public string Name { get; set; }

        public static IndicationModel FromEntity(Indication entity)
        {
            return entity == null ? null : new IndicationModel()
            {
                Id = entity.Id,
                Key = entity.Key,
                Name = entity.Name
            };
        }
    }
}
