using MyFoodDoc.Application.Abstractions;
using MyFoodDoc.Application.Entites;
using System.Collections.Generic;

namespace MyFoodDoc.Application.EnumEntities
{
    public class Diet : AbstractEnumEntity
    {
        public ICollection<DietTarget> Targets { get; set; }
    }
}
