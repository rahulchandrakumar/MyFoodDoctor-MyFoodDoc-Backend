using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MyFoodDoc.Application.Entites;

namespace MyFoodDoc.Infrastructure.Persistence.Database.Configuration
{
    public class CouponConfiguration : IEntityTypeConfiguration<Coupon>
    {
        public void Configure(EntityTypeBuilder<Coupon> builder)
        {
            builder.ToTable("Coupons", "Coupon");
            builder.HasKey(o => o.Id);
            builder.Property(o => o.Code).IsRequired().HasMaxLength(50);
            builder.Property(o => o.Expiry);
            builder.Property(o => o.Redeemed);
        }
    }
}
