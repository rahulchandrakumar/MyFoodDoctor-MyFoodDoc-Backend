using MyFoodDoc.Application.Abstractions;
using System;
using System.Collections.Generic;
using System.Text;

namespace MyFoodDoc.Application.Entites.Abstractions
{
    public interface IDiaryEntity<TKey> : IAuditableEntity
    {
        public TKey UserId { get; set; }

        public User User { get; set; }

        public DateTime Date { get; set; }
    }
}
