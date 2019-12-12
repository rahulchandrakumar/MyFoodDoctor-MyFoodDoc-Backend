using MyFoodDoc.CMS.Application.Models;
using System;

namespace MyFoodDoc.CMS.Models.VM
{
    public class Promotion : VMBase.BaseModel<int>
    {
        public string Title { get; set; }
        public int InsuranceId { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public int CouponCount { get; set; }
        public int UsedCouponCount { get; set; }
        public bool Disabled { get; set; }

        // for uploading only
        public int? TempFileId { get; set; }

        public static Promotion FromModel(PromotionModel model)
        {
            return new Promotion()
            {
                Id = model.Id,
                Disabled = !model.IsActive,
                CouponCount = model.CouponCount,
                InsuranceId = model.InsuranceId,
                UsedCouponCount = model.UsedCouponCount,
                StartDate = model.StartDate,
                EndDate = model.EndDate,
                Title = model.Title
            };
        }

        public PromotionModel ToModel()
        {
            return new PromotionModel()
            {
                Id = this.Id,
                IsActive = !this.Disabled,
                CouponCount = this.CouponCount,
                InsuranceId = this.InsuranceId,
                UsedCouponCount = this.UsedCouponCount,
                StartDate = this.StartDate,
                EndDate = this.EndDate,
                TempFileId = this.TempFileId,
                Title = this.Title
            };
        }
    }
}
