using MyFoodDoc.Application.Abstractions;
using System;

namespace MyFoodDoc.Application.Entities.Abstractions
{
    public interface IDiaryEntity<TKey> : IAuditableEntity
    {
        public TKey UserId { get; set; }

        public User User { get; set; }

        public DateTime Date { get; set; }
    }
}
