using FluentValidation;

namespace MyFoodDoc.App.Application.Payloads.Diary
{
    public class ExercisePayloadValidation : AbstractValidator<ExercisePayload>
    {
        public ExercisePayloadValidation()
        {
            RuleFor(x => x.Date).NotEmpty();
            RuleFor(x => x.Time).NotEmpty();
            RuleFor(x => x.Duration).NotNull().GreaterThanOrEqualTo(0);
        }
    }
}
