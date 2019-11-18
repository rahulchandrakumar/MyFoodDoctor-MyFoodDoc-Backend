using FluentValidation;

namespace MyFoodDoc.App.Application.Payloads.User
{
    public class RegisterPayloadValidation : AbstractValidator<RegisterPayload>
    {
        public RegisterPayloadValidation()
        {
            RuleFor(x => x.Email).NotEmpty().EmailAddress();
            RuleFor(x => x.Password).NotEmpty();
            RuleFor(x => x.InsuranceId).NotNull();
        }
    }
}
