using MyFoodDoc.App.Application.Abstractions;
using MyFoodDoc.App.Application.Exceptions;
using MyFoodDoc.App.Application.Payloads.Coupon;
using MyFoodDoc.Application.Abstractions;
using MyFoodDoc.Application.Entites;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;

namespace MyFoodDoc.App.Application.Services
{
    public class CouponService : ICouponService
    {
        private readonly IApplicationContext _context;

        public CouponService(IApplicationContext context)
        {
            _context = context;
        }

        public async Task RedeemCouponAsync(string userId, CouponPayload payload, CancellationToken cancellationToken)
        {
            var user = await _context.Users
                .Where(x => x.Id == userId)
                .SingleOrDefaultAsync(cancellationToken);
            
            if (user == null)
            {
                throw new ArgumentException();
            }

            var validCode = _context.Coupons.SingleOrDefault(x => x.Code == payload.Code && x.IsActive && x.Redeemed == null && x.Expiry < DateTime.UtcNow && x.InsuranceId == user.InsuranceId);   
            if (validCode == null)
            {
                throw new NotFoundException(nameof(Coupon), payload.Code);
            }

            validCode.Redeemed = DateTime.UtcNow;
            validCode.RedeemedBy = user.Id;

            await _context.SaveChangesAsync(cancellationToken);
        }
    }
}
