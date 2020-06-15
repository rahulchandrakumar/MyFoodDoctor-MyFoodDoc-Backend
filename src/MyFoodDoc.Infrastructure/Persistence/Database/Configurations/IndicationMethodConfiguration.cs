using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MyFoodDoc.Application.Entites;

namespace MyFoodDoc.Infrastructure.Persistence.Database.Configurations
{
    public class IndicationMethodConfiguration : IEntityTypeConfiguration<IndicationMethod>
    {
        public void Configure(EntityTypeBuilder<IndicationMethod> builder)
        {
            builder.ToTable("IndicationMethods", "System");
            builder.HasKey(x => new { x.IndicationId, x.MethodId });

            builder.HasOne(x => x.Indication)
                .WithMany(x => x.Methods)
                .HasForeignKey(x => x.IndicationId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(x => x.Method)
                .WithMany(x => x.Indications)
                .HasForeignKey(x => x.MethodId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
