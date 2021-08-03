using MyFoodDoc.Application.Abstractions;
using MyFoodDoc.Application.Enums;
using System;

namespace MyFoodDoc.Application.Entities.Subscriptions
{
    public abstract class AbstractSubscription : AbstractEntity
    {
        public string UserId { get; set; }

        public SubscriptionType Type { get; set; }

        public DateTime LastSynchronized { get; set; }

        public bool IsValid { get; set; }

        public User User { get; set; }

        public DateTime? FirstSynchronized { get; set; }
    }
}
