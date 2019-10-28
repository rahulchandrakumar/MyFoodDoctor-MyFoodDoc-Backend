using System.Linq;
using FluentValidation;

namespace MyFoodDoc.App.Application.Payloads.User
{
    public class AnamnesisPayloadValidation : AbstractValidator<AnamnesisPayload>
    {
        public AnamnesisPayloadValidation()
        {
            RuleFor(x => x.Gender).NotEmpty().IsInEnum();
            RuleFor(x => x.Height).NotEmpty();
            RuleFor(x => x.Weight).NotEmpty();            
            RuleFor(m => m.Motivations).NotEmpty().Must(x => x.Any()).When(m => !m.Indications.Any());
            RuleFor(m => m.Indications).NotEmpty().Must(x => x.Any()).When(m => !m.Motivations.Any());
        }
    }
}
