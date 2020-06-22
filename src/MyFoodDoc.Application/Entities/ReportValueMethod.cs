using MyFoodDoc.Application.Entities.Methods;
using MyFoodDoc.Application.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace MyFoodDoc.Application.Entities
{
    public abstract class ReportValueMethod : ReportMethod
    {
        public virtual ValueMethodType Type { get; }

        public object Value { get; set; }
    }
}