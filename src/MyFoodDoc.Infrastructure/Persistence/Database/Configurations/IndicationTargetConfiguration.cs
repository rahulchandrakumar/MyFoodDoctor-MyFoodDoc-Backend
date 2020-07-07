using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MyFoodDoc.Application.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace MyFoodDoc.Infrastructure.Persistence.Database.Configurations
{
    public class IndicationTargetConfiguration : IEntityTypeConfiguration<IndicationTarget>
    {
        public void Configure(EntityTypeBuilder<IndicationTarget> builder)
        {
            builder.ToTable("IndicationTargets", "System");
            builder.HasKey(x => new { x.IndicationId, x.TargetId });

            builder.HasOne(x => x.Indication)
                .WithMany(x => x.Targets)
                .HasForeignKey(x => x.IndicationId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(x => x.Target)
                .WithMany(x => x.Indications)
                .HasForeignKey(x => x.TargetId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
