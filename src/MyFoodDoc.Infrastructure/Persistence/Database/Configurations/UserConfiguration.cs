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

            builder.Property(x => x.Birthday).HasColumnType("Date");
            builder.Property(x => x.Gender).HasConversion<string>().HasMaxLength(6);
            builder.Property(x => x.Height).HasColumnType("decimal(4,1)");
            builder.Property(x => x.SubscriptionType).HasConversion<string>().HasMaxLength(15);
            builder.Property(x => x.PurchaseToken).HasColumnType("varchar(1000)");
            builder.Property(x => x.ReceiptData).HasColumnType("varchar(max)");
            builder.Property(x => x.ProductId).HasColumnType("varchar(1000)");
            builder.Property(x => x.OriginalTransactionId).HasColumnType("varchar(1000)");
            builder.Property(x => x.PushNotificationsEnabled).HasDefaultValue(false);
            builder.Property(x => x.DeviceToken).HasMaxLength(256);

            builder.HasIndex(x => x.Created);
            builder.HasIndex(x => x.PurchaseToken);
            builder.HasIndex(x => new { x.ProductId, x.OriginalTransactionId });
            builder.HasIndex(x => x.SubscriptionUpdated);

            builder.HasOne(x => x.Insurance).WithMany().HasForeignKey(x => x.InsuranceId);
            builder.HasMany(x => x.Indications).WithOne(x => x.User).OnDelete(DeleteBehavior.Cascade);
            builder.HasMany(x => x.Motivations).WithOne(x => x.User).OnDelete(DeleteBehavior.Cascade);
            builder.HasMany(x => x.Diets).WithOne(x => x.User).OnDelete(DeleteBehavior.Cascade);

            builder.HasMany(x => x.WeightHistory).WithOne(x => x.User).HasForeignKey(x => x.UserId);
            builder.HasMany(x => x.AbdominalGirthHistory).WithOne(x => x.User).HasForeignKey(x => x.UserId);
        }
    }
}
