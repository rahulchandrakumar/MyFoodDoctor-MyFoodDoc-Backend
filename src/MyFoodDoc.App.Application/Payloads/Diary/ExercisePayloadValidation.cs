using FluentValidation;

namespace MyFoodDoc.App.Application.Payloads.Diary
{
    public class ExercisePayloadValidation : AbstractValidator<ExercisePayload>
    {
        public ExercisePayloadValidation()
        {
            RuleFor(x => x.Duration).NotEmpty().GreaterThanOrEqualTo(1);
        }
    }
}
