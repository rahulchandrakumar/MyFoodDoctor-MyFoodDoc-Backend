using MyFoodDoc.Application.Abstractions;

namespace MyFoodDoc.Application.Entities.Courses
{
    public class UserAnswer : AbstractAuditableEntity
    {
        public string UserId { get; set; }

        public int ChapterId { get; set; }

        public bool Answer { get; set; }

        public User User { get; set; }

        public Chapter Chapter { get; set; }
    }
}
