using MyFoodDoc.Application.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace MyFoodDoc.Application.Entites.Targets
{
    public abstract class ValueTarget : Target
    {
        public virtual ValueTargetType Type { get; }
    }
}
