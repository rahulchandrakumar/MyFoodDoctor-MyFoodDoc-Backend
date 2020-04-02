using MyFoodDoc.Application.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace MyFoodDoc.Application.Entites.Targets
{
    public class SuggestionTarget : ValueTarget
    {
        public override ValueTargetType Type => ValueTargetType.Suggestion;
    }
}
