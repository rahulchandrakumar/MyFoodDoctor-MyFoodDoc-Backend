using FluentValidation;

namespace MyFoodDoc.App.Application.Payloads.Diary
{
    public class WeightHistoryPayloadValidation : AbstractValidator<WeightHistoryPayload>
    {
        public WeightHistoryPayloadValidation()
        {
            RuleFor(x => x.Date).NotEmpty();
            RuleFor(x => x.Value).NotNull().GreaterThanOrEqualTo(0);
        }
    }
}
