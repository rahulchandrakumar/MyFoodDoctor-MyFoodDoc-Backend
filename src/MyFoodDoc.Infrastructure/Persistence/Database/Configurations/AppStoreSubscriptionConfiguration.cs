using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MyFoodDoc.Application.Entities.Subscriptions;
using MyFoodDoc.Infrastructure.Persistence.Database.Configurations.Abstractions;
using System;
using System.Collections.Generic;
using System.Text;

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
            builder.Property(x => x.SubscriptionId).IsRequired().HasColumnType("varchar(1000)");
            builder.Property(x => x.PurchaseToken).IsRequired().HasColumnType("varchar(1000)");

            builder.HasIndex(x => x.PurchaseToken);
            builder.HasIndex(x => x.LastSynchronized);

            builder.HasOne(x => x.User).WithMany().HasForeignKey(x => x.UserId).OnDelete(DeleteBehavior.Cascade);
        }
    }
}
