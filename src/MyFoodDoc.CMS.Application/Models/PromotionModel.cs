using MyFoodDoc.Application.Entites;
using System;

namespace MyFoodDoc.CMS.Application.Models
{
    public class PromotionModel : BaseModel<int>
    {
        public string Title { get; set; }
        public int InsuranceId { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public int CouponCount { get; set; }
        public int UsedCouponCount { get; set; }
        public bool IsActive { get; set; }

        // for uploading only
        public int? TempFileId { get; set; }

        public static PromotionModel FromEntity(Promotion entity)
        {
            return entity == null ? null : new PromotionModel()
            {
                Id = entity.Id,
                StartDate = entity.StartDate,
                EndDate = entity.EndDate,
                IsActive = entity.IsActive,
                Title = entity.Title,
                InsuranceId = entity.InsuranceId,
            };
        }

        public Promotion ToEntity()
        {
            return new Promotion()
            {
                StartDate = this.StartDate,
                EndDate = this.EndDate,
                InsuranceId = this.InsuranceId,
                IsActive = this.IsActive,
                Title = this.Title
            };
        }
    }
}
