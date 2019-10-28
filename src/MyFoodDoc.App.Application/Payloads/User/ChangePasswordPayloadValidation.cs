using FluentValidation;

namespace MyFoodDoc.App.Application.Payloads.User
{
    public class ChangePasswordPayloadValidation : AbstractValidator<ChangePasswordPayload>
    {
        public ChangePasswordPayloadValidation()
        {
            RuleFor(x => x).Must(x => x.OldPassword != x.NewPassword);
            RuleFor(x => x.OldPassword).NotEmpty();
            RuleFor(x => x.NewPassword).NotEmpty();
        }
    }
}
