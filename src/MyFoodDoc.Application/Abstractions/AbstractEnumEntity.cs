using System;
using System.Collections.Generic;
using System.Text;

namespace MyFoodDoc.Application.Abstractions
{
    public abstract class AbstractEnumEntity : AbstractAuditableEntity,  IEnumEntity
    {
        public virtual string Key { get; set; }

        public virtual string Name { get; set; }
    }
}
