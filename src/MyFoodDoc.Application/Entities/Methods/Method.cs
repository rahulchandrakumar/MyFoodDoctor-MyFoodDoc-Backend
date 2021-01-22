using System;
using MyFoodDoc.Application.Abstractions;
using System.Collections.Generic;
using MyFoodDoc.Application.Enums;

namespace MyFoodDoc.Application.Entities.Methods
{
    public class Method : AbstractAuditableEntity
    {
        public MethodType Type { get; set; }

        public string Title { get; set; }

        public string Text { get; set; }

        public int? ImageId { get; set; }

        public int? Frequency { get; set; }

        public int? ParentId { get; set; }

        public MethodFrequencyPeriod? FrequencyPeriod { get; set; }

        public int? TimeIntervalDay { get; set; }

        public int? TimeIntervalNight { get; set; }

        public bool IsActive { get; set; }

        public ICollection<TargetMethod> Targets { get; set; }

        public ICollection<MotivationMethod> Motivations { get; set; }

        public ICollection<IndicationMethod> Indications { get; set; }

        public ICollection<DietMethod> Diets { get; set; }

        public Image Image { get; set; }

        public Method Parent { get; set; }

        public ICollection<Method> Children { get; set; }
    }
}
