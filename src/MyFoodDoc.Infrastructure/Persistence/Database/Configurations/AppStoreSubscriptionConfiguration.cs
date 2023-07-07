using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MyFoodDoc.Application.Entities.Subscriptions;
using MyFoodDoc.Infrastructure.Persistence.Database.Configurations.Abstractions;

namespace MyFoodDoc.Infrastructure.Persistence.Database.Configurations
{
    public class AppStoreSubscriptionConfiguration : AbstractSubscriptionConfiguration<AppStoreSubscription>
    {
        public override void Configure(EntityTypeBuilder<AppStoreSubscription> builder)
        {
            builder.ToTable("AppStoreSubscriptions", "User");

            builder.Property(x => x.UserId).IsRequired().HasMaxLength(450);
            builder.Property(x => x.Type).HasConversion<string>().HasMaxLength(15);
            builder.Property(x => x.LastSynchronized).HasDefaultValueSql("getutcdate()");
            builder.Property(x => x.ReceiptData).IsRequired().HasColumnType("varchar(max)");
            builder.Property(x => x.ProductId).IsRequired().HasColumnType("varchar(1000)");
            builder.Property(x => x.OriginalTransactionId).IsRequired().HasColumnType("varchar(1000)");

            builder.HasIndex(x => new { x.ProductId, x.OriginalTransactionId });
            builder.HasIndex(x => x.LastSynchronized);

            builder.HasOne(x => x.User)
                .WithMany(x => x.AppStoreSubscriptions)
                .HasForeignKey(x => x.UserId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
