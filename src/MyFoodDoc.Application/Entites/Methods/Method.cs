using MyFoodDoc.Application.Abstractions;
using MyFoodDoc.Application.EnumEntities;
using System;
using System.Collections.Generic;
using System.Text;
using MyFoodDoc.Application.Entites.Targets;
using MyFoodDoc.Application.Enums;

namespace MyFoodDoc.Application.Entites.Abstractions
{
    public class Method : AbstractAuditableEntity
    {
        public int TargetId { get; set; }

        public MethodType Type { get; set; }

        public string Title { get; set; }

        public string Text { get; set; }

        public ICollection<MotivationMethod> Motivations { get; set; }

        public ICollection<IndicationMethod> Indications { get; set; }

        public ICollection<DietMethod> Diets { get; set; }

        public Target Target { get; set; }
    }
}
