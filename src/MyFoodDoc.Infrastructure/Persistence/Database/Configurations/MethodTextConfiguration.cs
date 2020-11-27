using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MyFoodDoc.Application.Entities.Methods;

namespace MyFoodDoc.Infrastructure.Persistence.Database.Configurations
{
    public class MethodTextConfiguration : IEntityTypeConfiguration<MethodText>
    {
        public virtual void Configure(EntityTypeBuilder<MethodText> builder)
        {
            builder.ToTable("MethodTexts", "System");

            builder.HasKey(o => o.Id);
            builder.Property(o => o.Id).IsRequired().ValueGeneratedOnAdd();
            builder.Property(o => o.Code).IsRequired().HasMaxLength(100);
            builder.Property(o => o.Title).IsRequired().HasMaxLength(100);
            builder.Property(o => o.Text).IsRequired().HasMaxLength(1000);

            builder.HasOne(x => x.Method).WithMany().HasForeignKey(x => x.MethodId).OnDelete(DeleteBehavior.Cascade);
        }
    }
}
