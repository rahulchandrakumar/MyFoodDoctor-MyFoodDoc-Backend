namespace MyFoodDoc.Application.Entities
{
    public class UserStatsDto
    {
        public SubscriptionStatsDto GoogleStats { get; set; }

        public SubscriptionStatsDto AppleStats { get; set; }

        public int ActiveUsers { get; set; }
    }
}
