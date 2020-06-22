using MyFoodDoc.Application.Entities.Targets;
using MyFoodDoc.Application.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace MyFoodDoc.Application.Entities
{
    public abstract class ReportValueTarget : ReportTarget
    {
        //public virtual ValueTargetType Type { get; }

        public object Value { get; set; }
    }
}