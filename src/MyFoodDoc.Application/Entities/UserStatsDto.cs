using System;

namespace MyFoodDoc.Application.Models
{
    public class SubscriptionStatsDto
    {
        public int TotalSubscriptions { get; set; }

        public int MonthlySubscriptions { get; set; }

        public int HalfYearlySubscriptions { get; set; }

        public int YearlySubscriptions { get; set; }
    }

    public class UserStatsDto
    {
        public SubscriptionStatsDto GoogleStats { get; set; }

        public SubscriptionStatsDto AppleStats { get; set; }

        public int ActiveUsers { get; set; }
    }
}
