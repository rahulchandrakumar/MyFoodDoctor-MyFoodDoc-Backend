using MyFoodDoc.Application.EnumEntities;

namespace MyFoodDoc.CMS.Application.Models
{
    public class DietModel : BaseModel<int>
    {
        public string Key { get; set; }

        public string Name { get; set; }

        public static DietModel FromEntity(Diet entity)
        {
            return entity == null ? null : new DietModel()
            {
                Id = entity.Id,
                Key = entity.Key,
                Name = entity.Name
            };
        }
    }
}
