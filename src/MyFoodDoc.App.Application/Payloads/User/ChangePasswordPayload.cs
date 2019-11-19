namespace MyFoodDoc.App.Application.Payloads.User
{
    public class ChangePasswordPayload
    {
        public string OldPassword { get; set; }

        public string NewPassword { get; set; }

        public string NewPasswordConfirmation { get; set; }
    }
}
