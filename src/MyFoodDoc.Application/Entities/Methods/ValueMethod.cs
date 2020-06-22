using MyFoodDoc.Application.Entities.Abstractions;
using MyFoodDoc.Application.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace MyFoodDoc.Application.Entities.Methods
{
    public abstract class ValueMethod : Method
    {
        public virtual ValueMethodType Type { get; }
    }
}
