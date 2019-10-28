using System.Threading.Tasks;

namespace MyFoodDoc.App.Application.Abstractions
{
    public interface ICouponService
    {
        Task<bool> RedeemCoupon(string Code);
    }
}
