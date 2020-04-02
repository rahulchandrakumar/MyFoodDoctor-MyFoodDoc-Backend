using MyFoodDoc.Application.Entites.Targets;
using MyFoodDoc.Application.EnumEntities;
using System;
using System.Collections.Generic;
using System.Text;

namespace MyFoodDoc.Application.Entites
{
    public abstract class ReportTarget
    {
        public int ReportId { get; set; }

        public int TargetId { get; set; }

        public DateTime ResponseDate { get; set; }

        public Report Report { get; protected set; }

        public Target Target { get; protected set; }
    }
}
