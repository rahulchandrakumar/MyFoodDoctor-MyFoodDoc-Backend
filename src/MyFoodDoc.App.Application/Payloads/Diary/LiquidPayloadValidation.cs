using FluentValidation;

namespace MyFoodDoc.App.Application.Payloads.Diary
{
    public class LiquidPayloadValidation : AbstractValidator<LiquidPayload>
    {
        public LiquidPayloadValidation()
        {
            RuleFor(x => x.Amount).NotEmpty().GreaterThanOrEqualTo(1);
        }
    }
}
