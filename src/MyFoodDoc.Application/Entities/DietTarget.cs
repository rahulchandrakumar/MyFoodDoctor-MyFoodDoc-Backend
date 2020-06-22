using MyFoodDoc.Application.Entities.Targets;
using MyFoodDoc.Application.EnumEntities;
using System;
using System.Collections.Generic;
using System.Text;

namespace MyFoodDoc.Application.Entities
{
    public class DietTarget
    {
        public int DietId { get; set; }

        public int TargetId { get; set; }

        public Diet Diet { get; set; }

        public Target Target { get; set; }
    }
}
