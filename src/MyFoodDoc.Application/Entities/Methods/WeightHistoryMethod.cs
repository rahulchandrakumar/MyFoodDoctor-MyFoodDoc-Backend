using MyFoodDoc.Application.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace MyFoodDoc.Application.Entities.Methods
{
    public class WeightHistoryMethod : ValueMethod
    {
        public override ValueMethodType Type => ValueMethodType.WeightHistory;
    }
}
