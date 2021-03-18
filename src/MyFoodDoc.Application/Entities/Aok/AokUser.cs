namespace MyFoodDoc.Application.Entities.Aok
{
    public class AokUser
    {
        public string UserId { get; set; }
        public string Token { get; set; }

        public User User { get; set; }
    }
}