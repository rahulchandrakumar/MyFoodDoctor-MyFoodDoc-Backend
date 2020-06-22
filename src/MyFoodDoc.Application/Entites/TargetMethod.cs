using System;
using System.Collections.Generic;
using System.Text;
using MyFoodDoc.Application.Entites.Abstractions;
using MyFoodDoc.Application.Entites.Targets;

namespace MyFoodDoc.Application.Entites
{
    public class TargetMethod
    {
        public int TargetId { get; set; }

        public int MethodId { get; set; }

        public Target Target { get; set; }

        public Method Method { get; set; }
    }
}
