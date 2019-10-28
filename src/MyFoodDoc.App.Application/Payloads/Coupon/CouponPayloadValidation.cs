using FluentValidation;

namespace MyFoodDoc.App.Application.Payloads.Coupon
{
    public class CouponPayloadValidation : AbstractValidator<CouponPayload>
    {
        public CouponPayloadValidation()
        {
            RuleFor(x => x.Code).NotEmpty();
        }
    }
}
