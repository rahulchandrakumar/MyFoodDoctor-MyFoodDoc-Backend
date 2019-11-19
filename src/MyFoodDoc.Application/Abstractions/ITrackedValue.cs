using System;
using System.Collections.Generic;
using System.Text;

namespace MyFoodDoc.Application.Abstractions
{
    public interface ITrackedValue<TValue>
    {
        public DateTime Date { get; set; }

        public TValue Value { get; set; }
    }
}
