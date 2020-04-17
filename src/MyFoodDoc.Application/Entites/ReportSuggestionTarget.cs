using MyFoodDoc.Application.Entites.Targets;
using MyFoodDoc.Application.Enums;
using System;

namespace MyFoodDoc.Application.Entites
{
    public class ReportSuggestionTarget : ReportValueTarget
    {
        //public override ValueTargetType Type { get; } = ValueTargetType.Suggestion;

        public new bool? Value => IsAccepted;

        //public new SuggestionTarget Target { get; protected set; }

        public bool? IsAccepted { get; set; }
    }
}
