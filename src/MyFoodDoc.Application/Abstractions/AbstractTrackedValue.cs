using System;
using System.Collections.Generic;
using System.Text;

namespace MyFoodDoc.Application.Abstractions
{
    public abstract class AbstractTrackedValue<TValue> : ITrackedValue<TValue>
    {
        public virtual DateTime Date { get; set; }

        public virtual TValue Value { get; set; }
    }
}
