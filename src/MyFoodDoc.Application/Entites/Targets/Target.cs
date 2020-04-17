using MyFoodDoc.Application.Abstractions;
using MyFoodDoc.Application.EnumEntities;
using MyFoodDoc.Application.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace MyFoodDoc.Application.Entites.Targets
{
    public class Target : AbstractAuditableEntity
    {
        public int OptimizationAreaId { get; set; }

        public int TriggerId { get; set; }

        public int Threshold { get; set; }

        public TargetPriority Priority { get; set; }

        public string Title { get; set; }

        public string Text { get; set; }

        public TargetType Type { get; set; }

        public OptimizationArea OptimizationArea { get; set; }

        public Trigger Trigger { get; set; }

        public ICollection<MotivationTarget> Motivations { get; set; }

        public ICollection<IndicationTarget> Indications { get; set; }

        public ICollection<DietTarget> Diets { get; set; }
    }
}
