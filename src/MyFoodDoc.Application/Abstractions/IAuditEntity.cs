using System;

namespace MyFoodDoc.Application.Abstractions
{
    public interface IAuditEntity<TKey> : IEntity<TKey>
    {
        DateTime Created { get; set; }

        DateTime? LastModified { get; set; }
    }
}
