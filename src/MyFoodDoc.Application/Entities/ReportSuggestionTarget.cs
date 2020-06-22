using MyFoodDoc.Application.Entities.Targets;
using MyFoodDoc.Application.Enums;
using System;

namespace MyFoodDoc.Application.Entities
{
    public class ReportSuggestionTarget : ReportValueTarget
    {
        //public override ValueTargetType Type { get; } = ValueTargetType.Suggestion;

        public new bool? Value => IsAccepted;

        //public new SuggestionTarget Target { get; protected set; }

        public bool? IsAccepted { get; set; }
    }
}
