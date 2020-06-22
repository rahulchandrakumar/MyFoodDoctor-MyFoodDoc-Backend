using MyFoodDoc.Application.Abstractions;
using System;
using System.Collections.Generic;

namespace MyFoodDoc.Application.Entities
{
    public class Promotion : AbstractAuditableEntity
    {
        public string Title { get; set; }

        public int InsuranceId { get; set; }

        public Insurance Insurance { get; set; }

        public bool IsActive { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }

        public ICollection<Coupon> Coupons { get; set; }
    }
}
