using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MyFoodDoc.Application.Entities;
using MyFoodDoc.Application.Entities.Targets;
using MyFoodDoc.Application.EnumEntities;
using System;
using System.Collections.Generic;
using System.Text;

namespace MyFoodDoc.Infrastructure.Persistence.Database.Configurations
{
    public class AdjustmentTargetConfiguration : IEntityTypeConfiguration<AdjustmentTarget>
    {
        public virtual void Configure(EntityTypeBuilder<AdjustmentTarget> builder)
        {
            builder.ToTable("AdjustmentTargets", "System");

            builder.HasKey(o => o.Id);
            builder.Property(o => o.Id).IsRequired().ValueGeneratedOnAdd();
            builder.Property(o => o.TargetValue).IsRequired();
            builder.Property(o => o.Step).IsRequired();
            builder.Property(o => o.StepDirection).HasConversion<string>().HasMaxLength(10);
            builder.Property(o => o.RecommendedText).IsRequired().HasMaxLength(1000);
            builder.Property(o => o.TargetText).IsRequired().HasMaxLength(1000);
            builder.Property(o => o.RemainText).IsRequired().HasMaxLength(1000);

            builder.HasOne(x => x.Target).WithMany().HasForeignKey(x => x.TargetId).OnDelete(DeleteBehavior.Cascade);
        }
    }
}
