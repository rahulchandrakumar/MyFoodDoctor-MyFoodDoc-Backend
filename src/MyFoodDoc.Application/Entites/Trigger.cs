using MyFoodDoc.Application.Abstractions;
using MyFoodDoc.Application.EnumEntities;
using MyFoodDoc.Application.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace MyFoodDoc.Application.Entites
{
    public class Trigger : AbstractAuditableEntity
    {
        public int OptimizationAreaId { get; set; }

        public TriggerOperator Operator { get; set; }

        public int Value { get; set; }

        public OptimizationArea OptimizationArea { get; set; }
    }
}
