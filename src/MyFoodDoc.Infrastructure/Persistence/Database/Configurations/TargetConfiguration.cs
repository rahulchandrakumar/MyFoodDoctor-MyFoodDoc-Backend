using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MyFoodDoc.Application.Entites;
using MyFoodDoc.Application.Entites.Targets;
using MyFoodDoc.Application.EnumEntities;
using System;
using System.Collections.Generic;
using System.Text;

namespace MyFoodDoc.Infrastructure.Persistence.Database.Configurations
{
    public class TargetConfiguration : IEntityTypeConfiguration<Target>
    {
        public virtual void Configure(EntityTypeBuilder<Target> builder)
        {
            builder.ToTable("Targets", "System");

            builder.HasKey(o => o.Id);
            builder.Property(o => o.Id).IsRequired().ValueGeneratedOnAdd();
            builder.Property(o => o.TriggerOperator).HasConversion<string>().HasMaxLength(11);
            builder.Property(o => o.TriggerValue).IsRequired();
            builder.Property(o => o.Threshold).IsRequired();
            builder.Property(o => o.Priority).HasConversion<string>().HasMaxLength(6);
            builder.Property(o => o.Title).IsRequired().HasMaxLength(1000);
            builder.Property(o => o.Text).IsRequired().HasMaxLength(1000);
            builder.Property(o => o.Type).HasConversion<string>().HasMaxLength(10);

            builder.HasOne(x => x.OptimizationArea).WithMany().HasForeignKey(x => x.OptimizationAreaId).OnDelete(DeleteBehavior.Cascade);
        }
    }
}
