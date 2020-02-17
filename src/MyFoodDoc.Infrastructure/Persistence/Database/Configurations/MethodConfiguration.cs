using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MyFoodDoc.Application.Entites.Abstractions;

namespace MyFoodDoc.Infrastructure.Persistence.Database.Configuration.Abstractions
{
    public abstract class MethodConfiguration<TMethod> : IEntityTypeConfiguration<TMethod> where TMethod : Method
    {
        public virtual void Configure(EntityTypeBuilder<TMethod> builder)
        {
            builder.HasKey(method => method.Id);
            builder.Property(method => method.Id).IsRequired().ValueGeneratedOnAdd();
            builder.Property(method => method.AnalysisFlagId).IsRequired();
            builder.Property(method => method.OptimizationAreaId).IsRequired();
            builder.Property(method => method.Text).IsRequired();

            builder.HasOne(method => method.Flag).WithMany().HasForeignKey(method => method.AnalysisFlagId);
            builder.HasOne(method => method.OptimizationArea).WithMany().HasForeignKey(method => method.OptimizationAreaId);
        }
    }
}
