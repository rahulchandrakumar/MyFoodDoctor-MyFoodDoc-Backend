using FluentValidation;

namespace MyFoodDoc.App.Application.Payloads.User
{
    public class ForgotPasswordPayloadValidation : AbstractValidator<ForgotPasswordPayload>
    {
        public ForgotPasswordPayloadValidation()
        {
            RuleFor(x => x.Email).NotEmpty().EmailAddress();
        }
    }
}