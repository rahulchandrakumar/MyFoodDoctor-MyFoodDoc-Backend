using System;

namespace MyFoodDoc.Application.Abstractions
{
    public class AbstractAuditableEntity : AbstractEntity, IAuditableEntity
    {
        public virtual DateTime Created { get; set; }
        public virtual DateTime? LastModified { get; set; }
    }
}
