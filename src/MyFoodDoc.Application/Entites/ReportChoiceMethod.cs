using MyFoodDoc.Application.Entites.Methods;
using MyFoodDoc.Application.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace MyFoodDoc.Application.Entites
{
    public abstract class ReportChoiceMethod : ReportMethod
    {
        public virtual ChoiceMethodType Type { get; }

        public IEnumerable<ReportChoiceMethodChoice> Choices { get; set; }
    }
}