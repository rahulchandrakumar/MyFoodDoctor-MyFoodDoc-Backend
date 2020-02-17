using MyFoodDoc.Application.Abstractions;
using MyFoodDoc.Application.EnumEntities;
using System;
using System.Collections.Generic;
using System.Text;

namespace MyFoodDoc.Application.Entites.Abstractions
{
    public abstract class Method : AbstractAuditableEntity
    {
        public int OptimizationAreaId { get; set; }

        public int AnalysisFlagId { get; set; }

        public string Title { get; set; }

        public string Text { get; set; }

        public OptimizationArea OptimizationArea { get; set; }

        public AnalysisFlag Flag { get; set; }
    }
}
