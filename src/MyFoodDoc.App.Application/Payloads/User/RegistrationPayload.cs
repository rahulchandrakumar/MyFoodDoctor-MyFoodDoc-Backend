namespace MyFoodDoc.App.Application.Payloads.User
{
    public class RegistrationPayload
    {
        public string Email { get; set; }

        public string Password { get; set; }

        public int InsuranceId { get; set; }
    }
}
