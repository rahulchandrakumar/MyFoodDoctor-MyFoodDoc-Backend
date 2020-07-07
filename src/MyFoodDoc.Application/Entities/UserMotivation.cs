using MyFoodDoc.Application.EnumEntities;

namespace MyFoodDoc.Application.Entities
{
    public class UserMotivation
    {
        public string UserId { get; set; }

        public int MotivationId { get; set; }

        public User User { get; set; }

        public Motivation Motivation { get; set; }
    }
}