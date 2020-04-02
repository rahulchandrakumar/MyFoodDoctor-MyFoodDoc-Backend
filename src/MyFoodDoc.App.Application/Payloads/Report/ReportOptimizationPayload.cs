using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace MyFoodDoc.App.Application.Payloads.Report
{
    public class ReportOptimizationPayload : List<ReportOptimizationPayload.Selection>
    {
        public class Selection
        {
            public int Id { get; set; }

            public object Value { get; set; }
        }
    }
}
