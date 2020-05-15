using System;
using System.Collections.Generic;
using System.Text;

namespace MyFoodDoc.App.Application.Models
{
    public class AnalysisDto
    {
        public decimal? UpperLimit { get; set; }

        public decimal? LowerLimit { get; set; }

        public decimal? Optimal { get; set; }

        public ICollection<AnalysisDataDto> Data { get; set; }
    }
}
