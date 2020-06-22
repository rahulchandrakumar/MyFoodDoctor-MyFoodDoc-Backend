using MyFoodDoc.Application.Abstractions;
using System;
using System.Collections.Generic;
using System.Text;

namespace MyFoodDoc.Application.Entities.Abstractions
{
    public class AbstractDiaryEntity<TKey> : AbstractAuditableEntity, IDiaryEntity<TKey>
    {
        public virtual TKey UserId { get; set; }

        public virtual DateTime Date { get; set; }

        public virtual User User { get; set; }
    }
}
