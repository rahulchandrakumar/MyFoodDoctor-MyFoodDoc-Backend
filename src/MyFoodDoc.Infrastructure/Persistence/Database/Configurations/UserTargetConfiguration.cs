using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MyFoodDoc.Application.Entities;
using MyFoodDoc.Application.Entities.Targets;
using System;
using System.Collections.Generic;
using System.Text;

namespace MyFoodDoc.Infrastructure.Persistence.Database.Configurations
{
    public class UserTargetConfiguration : IEntityTypeConfiguration<UserTarget>
    {
        public virtual void Configure(EntityTypeBuilder<UserTarget> builder)
        {
            builder.ToTable("UserTargets", "System");

            builder.HasKey(o => o.Id);
            builder.Property(o => o.Id).IsRequired().ValueGeneratedOnAdd();
            builder.Property(o => o.UserId).IsRequired().HasMaxLength(450);

            builder.HasIndex(p => new { p.UserId, p.TargetId });

            builder.HasOne(x => x.User).WithMany().HasForeignKey(x => x.UserId).OnDelete(DeleteBehavior.Cascade);
            builder.HasOne(x => x.Target).WithMany().HasForeignKey(x => x.TargetId).OnDelete(DeleteBehavior.Cascade);
        }
    }
}
