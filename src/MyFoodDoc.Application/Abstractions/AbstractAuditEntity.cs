using System;

namespace MyFoodDoc.Application.Abstractions
{
    public class AbstractAuditEntity<TKey> : AbstractEntity<TKey>, IAuditEntity<TKey>
    {
        public DateTime Created { get; set; }
        public DateTime? LastModified { get; set; }
    }
}
