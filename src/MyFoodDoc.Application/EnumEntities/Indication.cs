using MyFoodDoc.Application.Abstractions;
using MyFoodDoc.Application.Entites;
using System.Collections.Generic;

namespace MyFoodDoc.Application.EnumEntities
{
    public class Indication : AbstractEnumEntity
    {
        public ICollection<IndicationTarget> Targets { get; set; }

        public ICollection<IndicationMethod> Methods { get; set; }
    }
}
