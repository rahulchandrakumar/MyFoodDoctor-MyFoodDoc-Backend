using MyFoodDoc.Application.Entites.Abstractions;
using MyFoodDoc.Application.EnumEntities;
using System;
using System.Collections.Generic;
using System.Text;

namespace MyFoodDoc.Application.Entites
{
    public abstract class ReportMethod
    {
        public int ReportId { get; set; }

        public int MethodId { get; set; }

        public DateTime Date { get; set; }

        public DateTime ResponseDate { get; set; }

        public Report Report { get; set; }
        
        public Method Method { get; set; }
    }
}
