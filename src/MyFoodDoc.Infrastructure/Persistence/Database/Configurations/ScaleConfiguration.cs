using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MyFoodDoc.Application.Entities.Psychogramm;

namespace MyFoodDoc.Infrastructure.Persistence.Database.Configurations
{
    public class ScaleConfiguration : IEntityTypeConfiguration<Scale>
    {
        public virtual void Configure(EntityTypeBuilder<Scale> builder)
        {
            builder.ToTable("Scales", "Psychogramm");

            builder.HasKey(o => o.Id);
            builder.Property(o => o.Id).IsRequired().ValueGeneratedOnAdd();
            builder.Property(o => o.Title).IsRequired().HasMaxLength(100);
            builder.Property(o => o.Text).IsRequired().HasMaxLength(1000);
            builder.Property(o => o.Order).IsRequired();
            builder.Property(o => o.ImageId).IsRequired();

            builder.HasOne(x => x.Image).WithMany().HasForeignKey(x => x.ImageId).OnDelete(DeleteBehavior.SetNull);
        }
    }
}
