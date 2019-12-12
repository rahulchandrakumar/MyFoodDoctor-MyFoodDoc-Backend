using MyFoodDoc.Application.Abstractions;
using System;
using System.Collections.Generic;
using System.Text;

namespace MyFoodDoc.Application.Entites.TrackedValus
{
    public class AbstractUserTrackedValue<TValue> : AbstractTrackedValue<TValue>
    {
        public string UserId { get; set; }

        public User User { get; set; }
    }
}
