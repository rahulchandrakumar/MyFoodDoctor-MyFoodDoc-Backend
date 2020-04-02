using MyFoodDoc.Application.Entites.Methods;
using MyFoodDoc.Application.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace MyFoodDoc.Application.Entites
{
    public class ReportWeightHistoryMethod : ReportValueMethod
    {
        public override ValueMethodType Type { get; } = ValueMethodType.WeightHistory;

        public new WeightHistoryMethod Method { get; protected set; }

        public decimal? Weight { get; set; }
    }
}
