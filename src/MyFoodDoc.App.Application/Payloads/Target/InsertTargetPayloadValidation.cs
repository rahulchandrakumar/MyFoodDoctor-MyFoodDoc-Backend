using FluentValidation;

namespace MyFoodDoc.App.Application.Payloads.Target
{
    public class InsertTargetPayloadValidation : AbstractValidator<InsertTargetPayload>
    {
        public InsertTargetPayloadValidation()
        {
            RuleForEach(x => x.Targets).ChildRules(i =>
            {
                i.RuleFor(x => x.UserAnswerCode).NotNull();
            });
        }
    }
}
