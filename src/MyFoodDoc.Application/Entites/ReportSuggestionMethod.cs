using MyFoodDoc.Application.Entites.Methods;
using MyFoodDoc.Application.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace MyFoodDoc.Application.Entites
{
    public class ReportSuggestionMethod : ReportValueMethod
    {
        public override ValueMethodType Type { get; } = ValueMethodType.Suggestion;

        public new bool? Value => IsAccepted;

        public new SuggestionMethod Method { get; protected set; }

        public bool? IsAccepted { get; set; }
    }
}
