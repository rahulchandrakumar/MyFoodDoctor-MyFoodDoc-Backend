using System.Linq;
using FluentValidation;

namespace MyFoodDoc.App.Application.Payloads.Diary
{
    public class InsertMealPayloadValidation : AbstractValidator<InsertMealPayload>
    {
        public InsertMealPayloadValidation()
        {
            RuleFor(x => x.Time).NotEmpty();
            RuleFor(x => x.Type).NotEmpty().IsInEnum();
            RuleFor(x => x.Ingredients).NotEmpty().Must(x => x.Any());
            RuleFor(m => m.Mood).NotEmpty().InclusiveBetween(1,5);
        }
    }
}
