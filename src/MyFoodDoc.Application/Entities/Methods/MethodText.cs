using MyFoodDoc.Application.Abstractions;

namespace MyFoodDoc.Application.Entities.Methods
{
    public class MethodText : AbstractAuditableEntity
    {
        public int MethodId { get; set; }

        public string Code { get; set; }

        public string Title { get; set; }

        public string Text { get; set; }

        public Method Method { get; set; }
    }
}
