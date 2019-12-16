using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MyFoodDoc.Application.Entites.TrackedValus;

namespace MyFoodDoc.Infrastructure.Persistence.Database.Configuration
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
