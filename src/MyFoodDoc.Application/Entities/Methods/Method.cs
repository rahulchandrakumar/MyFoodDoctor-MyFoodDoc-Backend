using MyFoodDoc.Application.Abstractions;
using System.Collections.Generic;
using MyFoodDoc.Application.Entities.Targets;
using MyFoodDoc.Application.Enums;

namespace MyFoodDoc.Application.Entities.Abstractions
{
    public class Method : AbstractAuditableEntity
    {
        public int TargetId { get; set; }

        public MethodType Type { get; set; }

        public string Title { get; set; }

        public string Text { get; set; }

        public int? ImageId { get; set; }

        public ICollection<TargetMethod> Targets { get; set; }

        public ICollection<MotivationMethod> Motivations { get; set; }

        public ICollection<IndicationMethod> Indications { get; set; }

        public ICollection<DietMethod> Diets { get; set; }

        public Target Target { get; set; }

        public Image Image { get; set; }
    }
}
