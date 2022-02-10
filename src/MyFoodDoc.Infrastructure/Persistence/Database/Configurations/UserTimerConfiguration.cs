using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MyFoodDoc.Application.Entities;

namespace MyFoodDoc.Infrastructure.Persistence.Database.Configurations
{
    public class UserTimerConfiguration : IEntityTypeConfiguration<UserTimer>
    {
        public void Configure(EntityTypeBuilder<UserTimer> builder)
        {
            builder.ToTable("UserTimer", "System");

            builder.HasKey(x => x.UserId);

            builder.Property(x => x.UserId).IsRequired().HasMaxLength(450);

            builder.HasIndex(x => x.ExpirationDate);

            builder.HasOne(x => x.User).WithMany().HasForeignKey(x => x.UserId).OnDelete(DeleteBehavior.Cascade);
            builder.HasOne(x => x.Method).WithMany().HasForeignKey(x => x.MethodId).OnDelete(DeleteBehavior.Cascade);
        }
    }
}
