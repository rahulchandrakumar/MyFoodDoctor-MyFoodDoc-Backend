using MyFoodDoc.Application.Entities.Methods;
using MyFoodDoc.Application.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace MyFoodDoc.Application.Entities
{
    public abstract class ReportBasicChoiceMethod : ReportChoiceMethod
    {
        public override ChoiceMethodType Type { get; } = ChoiceMethodType.Basic;

        public new BasicChoiceMethod Method { get; protected set; }
    }
}