using MyFoodDoc.Application.Entites.Methods;
using MyFoodDoc.Application.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace MyFoodDoc.Application.Entites
{
    public abstract class ReportBasicChoiceMethod : ReportChoiceMethod
    {
        public override ChoiceMethodType Type { get; } = ChoiceMethodType.Basic;

        public new BasicChoiceMethod Method { get; protected set; }
    }
}