﻿using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MyFoodDoc.Application.Entities.Methods;

namespace MyFoodDoc.Infrastructure.Persistence.Database.Configurations
{
    public class MethodConfiguration : IEntityTypeConfiguration<Method>
    {
        public virtual void Configure(EntityTypeBuilder<Method> builder)
        {
            builder.ToTable("Methods", "System");

            builder.HasKey(o => o.Id);
            builder.Property(o => o.Id).IsRequired().ValueGeneratedOnAdd();
            builder.Property(o => o.Title).IsRequired().HasMaxLength(100);
            builder.Property(o => o.Text).IsRequired().HasMaxLength(1000);
            builder.Property(o => o.Type).HasConversion<string>().HasMaxLength(14);
            builder.Property(o => o.FrequencyPeriod).HasConversion<string>().HasMaxLength(5);

            builder.HasOne(x => x.Image).WithMany().HasForeignKey(x => x.ImageId).OnDelete(DeleteBehavior.SetNull);
        }
    }
}