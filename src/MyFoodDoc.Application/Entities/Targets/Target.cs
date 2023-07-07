using System.Collections;
using MyFoodDoc.Application.Abstractions;
using MyFoodDoc.Application.EnumEntities;
using MyFoodDoc.Application.Enums;
using System.Collections.Generic;

namespace MyFoodDoc.Application.Entities.Targets
{
    public class Target : AbstractAuditableEntity
    {
        public int OptimizationAreaId { get; set; }

        public TriggerOperator TriggerOperator { get; set; }

        public decimal TriggerValue { get; set; }

        public int Threshold { get; set; }

        public TargetPriority Priority { get; set; }

        public string Title { get; set; }

        public string Text { get; set; }

        public TargetType Type { get; set; }

        public int ImageId { get; set; }

        public OptimizationArea OptimizationArea { get; set; }

        public ICollection<TargetMethod> Methods { get; set; }

        public ICollection<MotivationTarget> Motivations { get; set; }

        public ICollection<IndicationTarget> Indications { get; set; }

        public ICollection<DietTarget> Diets { get; set; }

        public ICollection<AdjustmentTarget> AdjustmentTargets { get; set; }
        public Image Image { get; set; }
    }
}
