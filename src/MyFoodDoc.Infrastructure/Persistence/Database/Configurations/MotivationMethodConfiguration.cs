using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MyFoodDoc.Application.Entities;

namespace MyFoodDoc.Infrastructure.Persistence.Database.Configurations
{
    public class MotivationMethodConfiguration : IEntityTypeConfiguration<MotivationMethod>
    {
        public void Configure(EntityTypeBuilder<MotivationMethod> builder)
        {
            builder.ToTable("MotivationMethods", "System");
            builder.HasKey(x => new { x.MotivationId, x.MethodId });
            builder.Property(x => x.IsContraindication).HasDefaultValue(false);

            builder.HasOne(x => x.Motivation)
                .WithMany(x => x.Methods)
                .HasForeignKey(x => x.MotivationId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(x => x.Method)
                .WithMany(x => x.Motivations)
                .HasForeignKey(x => x.MethodId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
