using MyFoodDoc.Application.Abstractions;

namespace MyFoodDoc.Application.Entities.TrackedValues
{
    public class AbstractUserTrackedValue<TValue> : AbstractTrackedValue<TValue>
    {
        public string UserId { get; set; }

        public User User { get; set; }
    }
}
