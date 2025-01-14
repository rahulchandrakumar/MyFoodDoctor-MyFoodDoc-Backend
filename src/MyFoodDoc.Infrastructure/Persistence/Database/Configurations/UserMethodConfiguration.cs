﻿using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MyFoodDoc.Application.Entities;

namespace MyFoodDoc.Infrastructure.Persistence.Database.Configurations
{
    public class UserMethodConfiguration : IEntityTypeConfiguration<UserMethod>
    {
        public virtual void Configure(EntityTypeBuilder<UserMethod> builder)
        {
            builder.ToTable("UserMethods", "System");

            builder.HasKey(o => o.Id);
            builder.Property(o => o.Id).IsRequired().ValueGeneratedOnAdd();
            builder.Property(o => o.UserId).IsRequired().HasMaxLength(450);
            builder.Property(o => o.DecimalValue).HasColumnType("decimal(4,1)");

            builder.HasIndex(p => new { p.UserId, p.MethodId });

            builder.HasOne(x => x.User).WithMany().HasForeignKey(x => x.UserId).OnDelete(DeleteBehavior.Cascade);
            builder.HasOne(x => x.Method).WithMany().HasForeignKey(x => x.MethodId).OnDelete(DeleteBehavior.Cascade);
        }
    }
}
