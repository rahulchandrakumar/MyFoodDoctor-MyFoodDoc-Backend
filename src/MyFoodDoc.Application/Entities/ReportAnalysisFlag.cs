using MyFoodDoc.Application.EnumEntities;
using System;
using System.Collections.Generic;
using System.Text;

namespace MyFoodDoc.Application.Entities
{
    public class ReportAnalysisFlag
    {
        public int ReportId { get; set; }

        public int AnalysisFlagId { get; set; }

        public Report Report { get; set; }

        public AnalysisFlag Flag { get; set; }
    }
}
