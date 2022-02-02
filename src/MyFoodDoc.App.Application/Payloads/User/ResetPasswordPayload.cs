namespace MyFoodDoc.App.Application.Payloads.User
{
    public class ResetPasswordPayload
    {
        public string Email { get; set; }

        public string ResetToken { get; set; }

        public string NewPassword { get; set; }
    }
}
