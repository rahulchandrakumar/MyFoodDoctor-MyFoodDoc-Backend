using System;
using System.Collections.Generic;
using System.Text;

namespace MyFoodDoc.Application.Abstractions
{
    public interface IAuditableEntity : IEntity, IAuditable
    {
    }
}
