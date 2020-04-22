using MyFoodDoc.Application.Abstractions;
using MyFoodDoc.Application.Entites;

namespace MyFoodDoc.Application.EnumEntities
{
    public class OptimizationArea : AbstractEnumEntity
    {
        public string Text { get; set; }

        public int? ImageId { get; set; }

        public Image Image { get; set; }
    }
}
