using FluentValidation;

namespace MyFoodDoc.App.Application.Payloads.Diary
{
    public class FavouritePayloadValidation : AbstractValidator<FavouritePayload>
    {
        public FavouritePayloadValidation()
        {
            RuleFor(x => x.Title).NotEmpty();

            RuleFor(x => x.Ingredients).NotEmpty();

            RuleForEach(x => x.Ingredients).ChildRules(i =>
            {
                i.RuleFor(x => x.Amount).GreaterThanOrEqualTo(0);
            });
        }
    }
}
