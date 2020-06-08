using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MyFoodDoc.Application.Entites;

namespace MyFoodDoc.Infrastructure.Persistence.Database.Configurations
{
    public class UserMethodShowHistoryConfiguration : IEntityTypeConfiguration<UserMethodShowHistoryItem>
    {
        public virtual void Configure(EntityTypeBuilder<UserMethodShowHistoryItem> builder)
        {
            builder.ToTable("UserMethodShowHistory", "System");

            builder.HasKey(o => o.Id);
            builder.Property(o => o.Id).IsRequired().ValueGeneratedOnAdd();
            builder.Property(o => o.Date).HasDefaultValueSql("getutcdate()");
            builder.Property(o => o.UserId).IsRequired().HasMaxLength(450);
            
            builder.HasOne(x => x.User).WithMany().HasForeignKey(x => x.UserId).OnDelete(DeleteBehavior.Cascade);
            builder.HasOne(x => x.Method).WithMany().HasForeignKey(x => x.MethodId).OnDelete(DeleteBehavior.Cascade);
        }
    }
}
