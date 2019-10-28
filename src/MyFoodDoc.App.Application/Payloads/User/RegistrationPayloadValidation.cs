using FluentValidation;

namespace MyFoodDoc.App.Application.Payloads.User
{
    public class RegistrationPayloadValidation : AbstractValidator<RegistrationPayload>
    {
        public RegistrationPayloadValidation()
        {
            RuleFor(x => x.Email).NotEmpty().EmailAddress();
            RuleFor(x => x.Password).NotEmpty();
            RuleFor(x => x.InsuranceId).NotEmpty();
        }
    }
}
