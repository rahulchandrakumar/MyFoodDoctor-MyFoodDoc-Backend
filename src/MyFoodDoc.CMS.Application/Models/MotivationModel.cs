using MyFoodDoc.Application.EnumEntities;

namespace MyFoodDoc.CMS.Application.Models
{
    public class MotivationModel : BaseModel<int>
    {
        public string Key { get; set; }

        public string Name { get; set; }

        public static MotivationModel FromEntity(Motivation entity)
        {
            return entity == null ? null : new MotivationModel()
            {
                Id = entity.Id,
                Key = entity.Key,
                Name = entity.Name
            };
        }
    }
}
