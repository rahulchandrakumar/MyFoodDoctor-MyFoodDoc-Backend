using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MyFoodDoc.Application.Entites;
using System;
using System.Collections.Generic;
using System.Text;

namespace MyFoodDoc.Infrastructure.Persistence.Database.Configurations
{
    public class DietTargetConfiguration : IEntityTypeConfiguration<DietTarget>
    {
        public void Configure(EntityTypeBuilder<DietTarget> builder)
        {
            builder.ToTable("DietTargets", "System");
            builder.HasKey(x => new { x.DietId, x.TargetId });

            builder.HasOne(x => x.Diet)
                .WithMany(x => x.Targets)
                .HasForeignKey(x => x.DietId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(x => x.Target)
                .WithMany(x => x.Diets)
                .HasForeignKey(x => x.TargetId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
