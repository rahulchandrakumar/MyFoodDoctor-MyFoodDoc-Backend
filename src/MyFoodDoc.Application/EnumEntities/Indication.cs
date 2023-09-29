using MyFoodDoc.Application.Abstractions;
using MyFoodDoc.Application.Entities;
using System.Collections.Generic;

namespace MyFoodDoc.Application.EnumEntities
{
    public class Indication : AbstractEnumEntity
    {
        public const int DiabetesType1 = 3;
        public const int DiabetesType2 = 4;
        public const int EatingDisorder = 5;

        public ICollection<IndicationTarget> Targets { get; set; }

        public ICollection<IndicationMethod> Methods { get; set; }
    }
}
