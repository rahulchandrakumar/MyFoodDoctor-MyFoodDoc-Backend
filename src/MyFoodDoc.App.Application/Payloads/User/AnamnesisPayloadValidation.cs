using FluentValidation;
using System.Linq;

namespace MyFoodDoc.App.Application.Payloads.User
{
    public class AnamnesisPayloadValidation : AbstractValidator<AnamnesisPayload>
    {
        public AnamnesisPayloadValidation()
        {
            RuleFor(x => x.Gender).NotNull().IsInEnum();
            RuleFor(x => x.Height).NotEmpty();
            RuleFor(x => x.Weight).NotEmpty();
            RuleFor(m => m.Motivations).NotEmpty().When(m => m.Indications == null || !m.Indications.Any());
            RuleFor(m => m.Indications).NotEmpty().When(m => m.Motivations == null || !m.Motivations.Any());
            RuleFor(m => m.Diets);
        }
    }
}
