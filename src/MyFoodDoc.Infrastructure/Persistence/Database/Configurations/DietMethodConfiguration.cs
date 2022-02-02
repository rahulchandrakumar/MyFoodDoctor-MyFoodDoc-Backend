using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MyFoodDoc.Application.Entities;

namespace MyFoodDoc.Infrastructure.Persistence.Database.Configurations
{
    public class DietMethodConfiguration : IEntityTypeConfiguration<DietMethod>
    {
        public void Configure(EntityTypeBuilder<DietMethod> builder)
        {
            builder.ToTable("DietMethods", "System");
            builder.HasKey(x => new { x.DietId, x.MethodId });
            builder.Property(x => x.IsContraindication).HasDefaultValue(false);

            builder.HasOne(x => x.Diet)
                .WithMany(x => x.Methods)
                .HasForeignKey(x => x.DietId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(x => x.Method)
                .WithMany(x => x.Diets)
                .HasForeignKey(x => x.MethodId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
