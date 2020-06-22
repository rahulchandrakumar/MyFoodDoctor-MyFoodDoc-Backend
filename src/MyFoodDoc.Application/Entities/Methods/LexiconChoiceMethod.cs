using MyFoodDoc.Application.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace MyFoodDoc.Application.Entities.Methods
{
    public class LexiconChoiceMethod : ChoiceMethod
    {
        public override ChoiceMethodType Type => ChoiceMethodType.Lexicon;

        public LexiconEntry Lexicon { get; set; }
    }
}
