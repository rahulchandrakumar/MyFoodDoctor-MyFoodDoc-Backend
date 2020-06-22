using MyFoodDoc.Application.Abstractions;
using MyFoodDoc.Application.Entities;
using System.Collections.Generic;

namespace MyFoodDoc.Application.EnumEntities
{
    public class Motivation : AbstractEnumEntity
    {
        public ICollection<MotivationTarget> Targets { get; set; }

        public ICollection<MotivationMethod> Methods { get; set; }
    }
}
