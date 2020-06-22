using MyFoodDoc.Application.Abstractions;
using MyFoodDoc.Application.Entities;
using System.Collections.Generic;

namespace MyFoodDoc.Application.EnumEntities
{
    public class Diet : AbstractEnumEntity
    {
        public ICollection<DietTarget> Targets { get; set; }

        public ICollection<DietMethod> Methods { get; set; }
    }
}
