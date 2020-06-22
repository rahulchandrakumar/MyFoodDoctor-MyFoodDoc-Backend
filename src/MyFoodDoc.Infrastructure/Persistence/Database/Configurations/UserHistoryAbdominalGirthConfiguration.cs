using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MyFoodDoc.Application.Entities;
using MyFoodDoc.Application.Entities.TrackedValus;

namespace MyFoodDoc.Infrastructure.Persistence.Database.Configuration
{
    public class UserHistoryAbdominalGirthConfiguration : IEntityTypeConfiguration<UserAbdominalGirth>
    {
        public void Configure(EntityTypeBuilder<UserAbdominalGirth> builder)
        {
            builder.ToTable("AbdonimalGirthHistory", "User");
            
            builder.HasKey(x => new { x.UserId, x.Date });
            builder.Property(x => x.Date).HasColumnType("Date");
            builder.Property(x => x.Value).IsRequired().HasColumnType("decimal(4,1)");

            builder.HasOne(x => x.User).WithMany(e => e.AbdominalGirthHistory);
        }
    }
}
