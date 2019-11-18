using System;

namespace MyFoodDoc.Application.Abstractions
{
    public class AbstractAuditableEntity<TKey> : AbstractEntity<TKey>, IAuditableEntity<TKey>
    {
        public DateTime Created { get; set; }
        public DateTime? LastModified { get; set; }
    }
}
