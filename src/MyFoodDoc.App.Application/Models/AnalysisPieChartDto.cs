using System;
using System.Collections.Generic;
using System.Text;

namespace MyFoodDoc.App.Application.Models
{
    public class AnalysisPieChartDto
    {
        public string Title { get; set; }

        public string Text { get; set; }

        public ICollection<AnalysisPieChartDataDto> Data { get; set; }
    }
}
