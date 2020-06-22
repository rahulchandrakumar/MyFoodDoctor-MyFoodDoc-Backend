using MyFoodDoc.Application.Entities.Methods;
using MyFoodDoc.Application.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace MyFoodDoc.Application.Entities
{
    public class ReportMoodMethod : ReportValueMethod
    {
        public override ValueMethodType Type { get; } = ValueMethodType.Mood;

        public new MoodMethod Method { get; protected set; }

        public int? Mood { get; set; }
    }
}
