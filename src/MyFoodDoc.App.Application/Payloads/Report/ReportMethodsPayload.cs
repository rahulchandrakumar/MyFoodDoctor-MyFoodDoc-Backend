using System;
using System.Collections.Generic;
using System.Text;

namespace MyFoodDoc.App.Application.Payloads.Report
{
    public class ReportMethodsPayload : List<ReportMethodsPayload.Selection>
    {
        public class Selection
        {
            public int Id { get; set; }

            public object Value { get; set; }

            public IEnumerable<int> Choices { get; set; }
        }
    }
}
