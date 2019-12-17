using System.Linq;
using FluentValidation;

namespace MyFoodDoc.App.Application.Payloads.User
{
    public class UpdateUserPayloadValidation : AbstractValidator<UpdateUserPayload>
    {
        public UpdateUserPayloadValidation()
        {
            RuleFor(x => x.Birthday).NotEmpty().When(s => s.Birthday != null);
            RuleFor(x => x.Gender).IsInEnum();
            RuleFor(x => x.Height).NotEmpty();
            RuleFor(m => m.Indications).NotEmpty().When(m => m.Motivations == null || !m.Motivations.Any());
            RuleFor(m => m.Motivations).NotEmpty().When(m => m.Indications == null || !m.Indications.Any());
            RuleFor(m => m.Diets);
        }
    }
}
