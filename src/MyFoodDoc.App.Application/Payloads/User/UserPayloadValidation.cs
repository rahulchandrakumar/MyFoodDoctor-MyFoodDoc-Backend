using System.Linq;
using FluentValidation;

namespace MyFoodDoc.App.Application.Payloads.User
{
    public class UserPayloadValidation : AbstractValidator<UserPayload>
    {
        public UserPayloadValidation()
        {
            RuleFor(x => x.Birthday).NotEmpty();
            RuleFor(x => x.Gender).NotEmpty().IsInEnum();
            RuleFor(x => x.Height).NotEmpty();
            RuleFor(m => m.Indications).NotEmpty().When(m => !m.Indications.Any());
            RuleFor(m => m.Motivations).NotEmpty().When(m => !m.Motivations.Any());
            RuleFor(m => m.Diet).NotEmpty();
        }
    }
}
