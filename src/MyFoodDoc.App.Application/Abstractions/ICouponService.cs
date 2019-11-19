using MyFoodDoc.App.Application.Payloads.Coupon;
using System.Threading;
using System.Threading.Tasks;

namespace MyFoodDoc.App.Application.Abstractions
{
    public interface ICouponService
    {
        Task RedeemCouponAsync(string userId, CouponPayload payload, CancellationToken cancellationToken = default);
    }
}
