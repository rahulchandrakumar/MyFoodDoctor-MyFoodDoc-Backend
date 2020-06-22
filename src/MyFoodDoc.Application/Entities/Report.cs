using MyFoodDoc.Application.Abstractions;
using MyFoodDoc.Application.Entities.Methods;
using MyFoodDoc.Application.Entities.Targets;
using System;
using System.Collections.Generic;

namespace MyFoodDoc.Application.Entities
{
    public class Report : AbstractAuditableEntity
    {
        public string UserId { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }
        
        public User User { get; set; }

        public IEnumerable<Meal> Meals { get; set; }

        public IEnumerable<ReportAnalysisFlag> Flags { get; set; }

        public IEnumerable<ReportMethod> Methods { get; set; }

        public IEnumerable<ReportTarget> Targets { get; set; }
    }
}
