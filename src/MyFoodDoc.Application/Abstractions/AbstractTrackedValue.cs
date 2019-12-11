using MyFoodDoc.Application.Entites;
using System;
using System.Collections.Generic;
using System.Text;

namespace MyFoodDoc.Application.Abstractions
{
    public abstract class AbstractTrackedValue<TValue> : ITrackedValue<TValue>
    {
        public string UserId { get; set; }

        public virtual DateTime Date { get; set; }

        public virtual TValue Value { get; set; }

        public User User { get; set; }
    }
}
