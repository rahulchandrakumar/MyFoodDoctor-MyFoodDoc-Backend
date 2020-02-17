using MyFoodDoc.Application.Abstractions;
using MyFoodDoc.Application.Entites.Methods;
using MyFoodDoc.Application.Entites.Targets;
using System;
using System.Collections.Generic;

namespace MyFoodDoc.Application.Entites
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
