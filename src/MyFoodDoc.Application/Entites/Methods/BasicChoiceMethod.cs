using MyFoodDoc.Application.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace MyFoodDoc.Application.Entites.Methods
{
    public class BasicChoiceMethod : ChoiceMethod
    {
        public override ChoiceMethodType Type => ChoiceMethodType.Basic;
    }
}
