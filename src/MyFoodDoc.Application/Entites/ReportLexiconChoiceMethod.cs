using MyFoodDoc.Application.Entites.Methods;
using MyFoodDoc.Application.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace MyFoodDoc.Application.Entites
{
    public abstract class ReportLexiconChoiceMethod : ReportChoiceMethod
    {
        public override ChoiceMethodType Type { get; } = ChoiceMethodType.Lexicon;

        public new LexiconChoiceMethod Method { get; protected set; }
    }
}