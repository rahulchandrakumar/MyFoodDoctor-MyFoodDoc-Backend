using MyFoodDoc.Application.Abstractions;

namespace MyFoodDoc.Application.Entities.Psychogramm
{
    public class UserChoice : AbstractAuditableEntity
    {
        public string UserId { get; set; }

        public int ChoiceId { get; set; }

        public User User { get; set; }

        public Choice Choice { get; set; }
    }
}
