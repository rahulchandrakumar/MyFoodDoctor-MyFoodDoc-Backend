using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MyFoodDoc.Application.Entites;
using System;
using System.Collections.Generic;
using System.Text;

namespace MyFoodDoc.Infrastructure.Persistence.Database.Configurations
{
    public class MotivationTargetConfiguration : IEntityTypeConfiguration<MotivationTarget>
    {
        public void Configure(EntityTypeBuilder<MotivationTarget> builder)
        {
            builder.ToTable("MotivationTargets", "System");
            builder.HasKey(x => new { x.MotivationId, x.TargetId });

            builder.HasOne(x => x.Motivation)
                .WithMany(x => x.Targets)
                .HasForeignKey(x => x.MotivationId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(x => x.Target)
                .WithMany(x => x.Motivations)
                .HasForeignKey(x => x.TargetId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
