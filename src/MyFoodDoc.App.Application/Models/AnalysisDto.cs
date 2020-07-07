using System;
using System.Collections.Generic;
using System.Text;

namespace MyFoodDoc.App.Application.Models
{
    public class AnalysisDto
    {
        public AnalysisLineGraphDto LineGraph { get; set; }

        public AnalysisPieChartDto PieChart { get; set; }
    }
}
