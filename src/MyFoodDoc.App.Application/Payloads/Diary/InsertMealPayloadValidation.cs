using System.Linq;
using FluentValidation;

namespace MyFoodDoc.App.Application.Payloads.Diary
{
    public class InsertMealPayloadValidation : AbstractValidator<InsertMealPayload>
    {
        public InsertMealPayloadValidation()
        {
            RuleFor(x => x.Date).NotNull();
            RuleFor(x => x.Time).NotNull();
            RuleFor(x => x.Type).NotNull().IsInEnum();

            RuleForEach(x => x.Ingredients).ChildRules(i =>
            {
                i.RuleFor(x => x.Amount).GreaterThanOrEqualTo(0);
            });

            RuleForEach(x => x.Favourites).ChildRules(i =>
            {
                i.RuleFor(x => x.Title).NotEmpty();

                i.RuleFor(x => x.Ingredients).NotEmpty();

                i.RuleForEach(x => x.Ingredients).ChildRules(j =>
                {
                    j.RuleFor(x => x.Amount).GreaterThanOrEqualTo(0);
                });
            });

            RuleFor(m => m.Mood).InclusiveBetween(1,5);
        }
    }
}
