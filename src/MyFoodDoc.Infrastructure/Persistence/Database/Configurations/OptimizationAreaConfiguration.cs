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
            //builder.Property(p => p.ImageId).IsRequired();

            builder.Property(o => o.UpperLimit).HasColumnType("decimal(18,2)");
            builder.Property(o => o.LowerLimit).HasColumnType("decimal(18,2)");
            builder.Property(o => o.Optimal).HasColumnType("decimal(18,2)");

            builder.HasOne(x => x.Image).WithMany().HasForeignKey(x => x.ImageId).OnDelete(DeleteBehavior.SetNull);
        }
    }
}
