using System.Linq;
using FluentValidation;

namespace MyFoodDoc.App.Application.Payloads.Diary
{
    public class MealPayloadValidation : AbstractValidator<MealPayload>
    {
        public MealPayloadValidation()
        {
            RuleFor(x => x.Time).NotEmpty();
            RuleFor(x => x.Type).NotEmpty().IsInEnum();
            RuleFor(x => x.Ingredients).NotEmpty().Must(x => x.Any());
            RuleFor(m => m.Mood).NotEmpty().InclusiveBetween(1,5);
        }
    }
}
