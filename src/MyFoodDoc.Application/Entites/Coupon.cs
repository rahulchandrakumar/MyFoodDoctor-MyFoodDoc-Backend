using MyFoodDoc.Application.Abstractions;
using System;

namespace MyFoodDoc.Application.Entites
{
    public class Coupon : AbstractAuditableEntity
    {
        public string Code { get; set; }

        public DateTime? Expiry { get; set; }

        public DateTime? Redeemed { get; set; }

        public bool IsActive { get; set; }
    }
}
