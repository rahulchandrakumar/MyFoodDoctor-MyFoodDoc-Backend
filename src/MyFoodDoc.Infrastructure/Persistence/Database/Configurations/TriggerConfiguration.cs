using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MyFoodDoc.Application.Entites;
using MyFoodDoc.Application.EnumEntities;
using System;
using System.Collections.Generic;
using System.Text;

namespace MyFoodDoc.Infrastructure.Persistence.Database.Configurations
{
    public class TriggerConfiguration : IEntityTypeConfiguration<Trigger>
    {
        public virtual void Configure(EntityTypeBuilder<Trigger> builder)
        {
            builder.ToTable("Triggers", "System");

            builder.HasKey(o => o.Id);
            builder.Property(o => o.Id).IsRequired().ValueGeneratedOnAdd();
            builder.Property(o => o.Operator).HasConversion<string>().HasMaxLength(11);
            builder.Property(o => o.Value).IsRequired();

            builder.HasOne(x => x.OptimizationArea).WithMany().HasForeignKey(x => x.OptimizationAreaId).OnDelete(DeleteBehavior.Cascade);
        }
    }
}
