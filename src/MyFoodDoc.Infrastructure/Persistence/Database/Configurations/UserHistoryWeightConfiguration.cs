using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MyFoodDoc.Application.Entities.TrackedValues;

namespace MyFoodDoc.Infrastructure.Persistence.Database.Configurations
{
    public class UserHistoryWeightConfiguration : IEntityTypeConfiguration<UserWeight>
    {
        public void Configure(EntityTypeBuilder<UserWeight> builder)
        {
            builder.ToTable("WeightHistory", "User");

            builder.HasKey(x => new { x.UserId, x.Date });
            builder.Property(x => x.Date).HasColumnType("Date");
            builder.Property(x => x.Value).IsRequired().HasColumnType("decimal(4,1)");

            builder.HasOne(x => x.User).WithMany(e => e.WeightHistory);
        }
    }
}
