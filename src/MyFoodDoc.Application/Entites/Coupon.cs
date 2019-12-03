using MyFoodDoc.Application.Abstractions;
using System;

namespace MyFoodDoc.Application.Entites
{
    public class Coupon : AbstractAuditableEntity
    {
        public int PromotionId { get; set; }

        public Promotion Promotion { get; set; }

        public string Code { get; set; }

        public DateTime? Redeemed { get; set; }

        public string RedeemedBy { get; set; }

        public User Redeemer { get; set; }
    }
}
