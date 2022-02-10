using System;

namespace MyFoodDoc.Application.Abstractions
{
    public interface ITrackedValue<TValue>
    {
        public DateTime Date { get; set; }

        public TValue Value { get; set; }
    }
}
