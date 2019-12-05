using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MyFoodDoc.Application.Entites;

namespace MyFoodDoc.Infrastructure.Persistence.Database.Configurations
{
    public class PromotionConfiguration : IEntityTypeConfiguration<Promotion>
    {
        public void Configure(EntityTypeBuilder<Promotion> builder)
        {
            builder.ToTable("Promotions", "Coupon");
            builder.HasKey(o => new { o.Id, o.InsuranceId });
            builder.Property(o => o.Id).ValueGeneratedOnAdd();
            builder.Property(o => o.Title).HasMaxLength(100);
            builder.Property(o => o.StartDate).IsRequired();
            builder.Property(o => o.EndDate).IsRequired();
            builder.Property(o => o.IsActive);

            builder.HasOne(x => x.Insurance).WithMany().HasForeignKey(x => x.InsuranceId);
            builder.HasMany(x => x.Coupons).WithOne(x => x.Promotion).HasForeignKey(x => new { x.PromotionId, x.InsuranceId }).OnDelete(DeleteBehavior.Cascade);
        }
    }
}
