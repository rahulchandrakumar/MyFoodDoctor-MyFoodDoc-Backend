using FluentValidation;

namespace MyFoodDoc.App.Application.Payloads.Diary
{
    public class LiquidPayloadValidation : AbstractValidator<LiquidPayload>
    {
        public LiquidPayloadValidation()
        {
            RuleFor(x => x.Date).NotEmpty();
            RuleFor(x => x.Time).NotEmpty();
            RuleFor(x => x.Amount).NotNull().GreaterThanOrEqualTo(0);
        }
    }
}
