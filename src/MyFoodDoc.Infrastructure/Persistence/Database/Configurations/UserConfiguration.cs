using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MyFoodDoc.Application.Entities;

namespace MyFoodDoc.Infrastructure.Persistence.Database.Configurations
{
    public class UserConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.ToTable("Users", "User");
            builder.HasKey(x => x.Id);
            builder.HasIndex(x => new { x.UserName, x.Gender });
            builder.HasIndex(x => x.Created);
            builder.HasIndex(x => x.PurchaseToken);
            builder.HasIndex(x => x.SubscriptionUpdated);
            builder.Property(x => x.Birthday).HasColumnType("Date");
            builder.Property(x => x.Gender).HasConversion<string>().HasMaxLength(6);
            builder.Property(x => x.Height).HasColumnType("decimal(4,1)");
            builder.Property(x => x.SubscriptionType).HasConversion<string>().HasMaxLength(15);
            builder.Property(o => o.PurchaseToken).HasColumnType("varchar(1000)");
            builder.Property(o => o.ReceiptData).HasColumnType("varchar(max)");
            builder.Property(x => x.PushNotificationsEnabled).HasDefaultValue(false);
            builder.Property(o => o.DeviceToken).HasMaxLength(256);

            builder.HasOne(x => x.Insurance).WithMany().HasForeignKey(x => x.InsuranceId);
            builder.HasMany(x => x.Indications).WithOne(x => x.User).OnDelete(DeleteBehavior.Cascade);
            builder.HasMany(x => x.Motivations).WithOne(x => x.User).OnDelete(DeleteBehavior.Cascade);
            builder.HasMany(x => x.Diets).WithOne(x => x.User).OnDelete(DeleteBehavior.Cascade);

            builder.HasMany(e => e.WeightHistory).WithOne(x => x.User).HasForeignKey(x => x.UserId);
            builder.HasMany(e => e.AbdominalGirthHistory).WithOne(x => x.User).HasForeignKey(x => x.UserId);
        }
    }
}
