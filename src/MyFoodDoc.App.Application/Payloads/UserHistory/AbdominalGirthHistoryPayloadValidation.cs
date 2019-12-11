using FluentValidation;

namespace MyFoodDoc.App.Application.Payloads.Diary
{
    public class AbdominalGirthHistoryPayloadValidation : AbstractValidator<AbdominalGirthHistoryPayload>
    {
        public AbdominalGirthHistoryPayloadValidation()
        {
            RuleFor(x => x.Date).NotEmpty();
            RuleFor(x => x.Value).NotNull().GreaterThanOrEqualTo(0);
        }
    }
}
