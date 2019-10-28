using FluentValidation;

namespace MyFoodDoc.App.Application.Payloads.User
{
    public class ResetPasswordPayloadValidation : AbstractValidator<ResetPasswordPayload>
    {
        public ResetPasswordPayloadValidation()
        {
            RuleFor(x => x.Email).NotEmpty().EmailAddress();
        }
    }
}
