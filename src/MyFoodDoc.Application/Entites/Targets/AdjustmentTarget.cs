using MyFoodDoc.Application.Entites.Abstractions;
using MyFoodDoc.Application.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace MyFoodDoc.Application.Entites.Targets
{
    public abstract class AdjustmentTarget : ValueTarget
    {
        public override ValueTargetType Type => ValueTargetType.Adjustment;
    }
}
