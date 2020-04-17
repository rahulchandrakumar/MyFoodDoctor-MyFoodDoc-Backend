using MyFoodDoc.Application.Entites.Targets;
using MyFoodDoc.Application.EnumEntities;
using System;
using System.Collections.Generic;
using System.Text;

namespace MyFoodDoc.Application.Entites
{
    public class MotivationTarget
    {
        public int MotivationId { get; set; }

        public int TargetId { get; set; }

        public Motivation Motivation { get; set; }

        public Target Target { get; set; }
    }
}
