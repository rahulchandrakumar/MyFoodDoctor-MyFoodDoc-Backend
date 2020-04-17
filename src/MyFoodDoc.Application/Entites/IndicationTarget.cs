using MyFoodDoc.Application.Entites.Targets;
using MyFoodDoc.Application.EnumEntities;
using System;
using System.Collections.Generic;
using System.Text;

namespace MyFoodDoc.Application.Entites
{
    public class IndicationTarget
    {
        public int IndicationId { get; set; }

        public int TargetId { get; set; }

        public Indication Indication { get; set; }

        public Target Target { get; set; }
    }
}
