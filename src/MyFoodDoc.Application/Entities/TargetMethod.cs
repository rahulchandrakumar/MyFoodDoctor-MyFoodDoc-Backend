using System;
using System.Collections.Generic;
using System.Text;
using MyFoodDoc.Application.Entities.Abstractions;
using MyFoodDoc.Application.Entities.Targets;

namespace MyFoodDoc.Application.Entities
{
    public class TargetMethod
    {
        public int TargetId { get; set; }

        public int MethodId { get; set; }

        public Target Target { get; set; }

        public Method Method { get; set; }
    }
}
