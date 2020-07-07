using MyFoodDoc.Application.EnumEntities;

namespace MyFoodDoc.Application.Entities
{
    public class UserDiet
    {
        public string UserId { get; set; }

        public int DietId { get; set; }

        public User User { get; set; }

        public Diet Diet { get; set; }
    }
}