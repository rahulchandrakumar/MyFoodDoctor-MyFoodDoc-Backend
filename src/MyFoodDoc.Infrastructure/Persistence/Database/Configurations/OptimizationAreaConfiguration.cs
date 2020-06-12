using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MyFoodDoc.Application.EnumEntities;
using MyFoodDoc.Infrastructure.Persistence.Database.Configuration.Abstractions;

namespace MyFoodDoc.Infrastructure.Persistence.Database.Configuration
{
    public class OptimizationAreaConfiguration : AbstractEnumConfiguration<OptimizationArea, int>
    {
        public override void Configure(EntityTypeBuilder<OptimizationArea> builder)
        {
            base.Configure(builder);

            builder.ToTable("OptimizationAreas", "System");

            builder.Property(o => o.Text).IsRequired().HasMaxLength(1000);

            builder.Property(o => o.OptimalLineGraphTitle).HasMaxLength(100);
            builder.Property(o => o.OptimalLineGraphText).HasMaxLength(1000);
            builder.Property(o => o.BelowOptimalLineGraphTitle).HasMaxLength(100);
            builder.Property(o => o.BelowOptimalLineGraphText).HasMaxLength(1000);
            builder.Property(o => o.AboveOptimalLineGraphTitle).HasMaxLength(100);
            builder.Property(o => o.AboveOptimalLineGraphText).HasMaxLength(1000);

            builder.Property(o => o.OptimalPieChartTitle).HasMaxLength(100);
            builder.Property(o => o.OptimalPieChartText).HasMaxLength(1000);
            builder.Property(o => o.BelowOptimalPieChartTitle).HasMaxLength(100);
            builder.Property(o => o.BelowOptimalPieChartText).HasMaxLength(1000);
            builder.Property(o => o.AboveOptimalPieChartTitle).HasMaxLength(100);
            builder.Property(o => o.AboveOptimalPieChartText).HasMaxLength(1000);
            
            builder.Property(o => o.LineGraphUpperLimit).HasColumnType("decimal(18,2)");
            builder.Property(o => o.LineGraphLowerLimit).HasColumnType("decimal(18,2)");
            builder.Property(o => o.LineGraphOptimal).HasColumnType("decimal(18,2)");

            builder.HasOne(x => x.Image).WithMany().HasForeignKey(x => x.ImageId).OnDelete(DeleteBehavior.SetNull);
        }
    }
}
