using MyFoodDoc.Application.Entites.Abstractions;
using MyFoodDoc.Application.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace MyFoodDoc.Application.Entites.Methods
{
    public abstract class ChoiceMethod : Method
    {
        public virtual ChoiceMethodType Type { get; }

        public IEnumerable<ChoiceMethodChoice> Choices { get; set; }
    }
}
