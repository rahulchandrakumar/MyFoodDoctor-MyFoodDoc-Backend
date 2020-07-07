using System;
using System.Collections.Generic;
using System.Text;

namespace MyFoodDoc.App.Application.Models
{
    public class OptimizationAreaDto
    {
        public string Key { get; set; }

        public string Name { get; set; }

        public string Text { get; set; }

        public string ImageUrl { get; set; }

        public AnalysisDto Analysis { get; set; }
        
        public ICollection<TargetDto> Targets { get; set; }
    }
}
