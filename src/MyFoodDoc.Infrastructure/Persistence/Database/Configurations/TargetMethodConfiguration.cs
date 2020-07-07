using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MyFoodDoc.Application.Entities;

namespace MyFoodDoc.Infrastructure.Persistence.Database.Configurations
{
    public class TargetMethodConfiguration : IEntityTypeConfiguration<TargetMethod>
    {
        public void Configure(EntityTypeBuilder<TargetMethod> builder)
        {
            builder.ToTable("TargetMethods", "System");
            builder.HasKey(x => new { x.TargetId, x.MethodId });

            builder.HasOne(x => x.Target)
                .WithMany(x => x.Methods)
                .HasForeignKey(x => x.TargetId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(x => x.Method)
                .WithMany(x => x.Targets)
                .HasForeignKey(x => x.MethodId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
