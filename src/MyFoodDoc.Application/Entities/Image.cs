using MyFoodDoc.Application.Abstractions;

namespace MyFoodDoc.Application.Entities
{
    public class Image : AbstractAuditableEntity
    {
        public string Url { get; set; }
    }
}
