using System.Linq;
using FluentValidation;

namespace MyFoodDoc.App.Application.Payloads.Diary
{
    public class UpdateMealPayloadValidation : AbstractValidator<UpdateMealPayload>
    {
        public UpdateMealPayloadValidation()
        {
            RuleFor(x => x.Time).NotNull();
            RuleFor(x => x.Type).NotNull().IsInEnum();

            RuleForEach(x => x.Ingredients).ChildRules(i =>
            {
                i.RuleFor(x => x.Amount).GreaterThanOrEqualTo(0);
            });

            RuleFor(m => m.Mood).InclusiveBetween(1,5);
        }
    }
}
