using MyFoodDoc.Application.Abstractions;
using MyFoodDoc.Application.Entities.Targets;

namespace MyFoodDoc.Application.Entities
{
    public class UserTarget : AbstractAuditableEntity
    {
        public string UserId { get; set; }

        public int TargetId { get; set; }

        public string TargetAnswerCode { get; set; }

        public User User { get; set; }

        public Target Target { get; set; }
    }
}
