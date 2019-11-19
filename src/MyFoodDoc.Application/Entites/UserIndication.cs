using MyFoodDoc.Application.EnumEntities;

namespace MyFoodDoc.Application.Entites
{
    public class UserIndication
    {
        public string UserId { get; set; }

        public int IndicationId { get; set; }

        public User User { get; set; }

        public Indication Indication { get; set; }
    }
}