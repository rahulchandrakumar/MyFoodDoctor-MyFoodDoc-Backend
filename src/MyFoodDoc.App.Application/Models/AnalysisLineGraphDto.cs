using System.Collections.Generic;

namespace MyFoodDoc.App.Application.Models
{
    public class AnalysisLineGraphDto
    {
        public string Title { get; set; }

        public string Text { get; set; }

        public decimal? UpperLimit { get; set; }

        public decimal? LowerLimit { get; set; }

        public decimal? Optimal { get; set; }

        public ICollection<AnalysisLineGraphDataDto> Data { get; set; }
    }
}
