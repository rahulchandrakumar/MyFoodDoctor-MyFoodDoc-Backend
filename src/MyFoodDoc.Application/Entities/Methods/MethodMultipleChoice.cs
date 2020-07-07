using MyFoodDoc.Application.Abstractions;

namespace MyFoodDoc.Application.Entities.Methods
{
    public class MethodMultipleChoice : AbstractAuditableEntity
    {
        public int MethodId { get; set; }

        public string Title { get; set; }

        public bool IsCorrect { get; set; }

        public Method Method { get; set; }
    }
}
