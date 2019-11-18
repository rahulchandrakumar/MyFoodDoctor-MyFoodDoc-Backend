using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MyFoodDoc.Application.EnumEntities;

namespace MyFoodDoc.Infrastructure.Persistence.Database.Configurations
{
    class IndicationConfiguration : IEntityTypeConfiguration<Indication>
    {
        public void Configure(EntityTypeBuilder<Indication> builder)
        {
            builder.ToTable("Indications", "Values");
            builder.HasKey(o => o.Id);
            builder.HasIndex(o => o.Key);
            builder.Property(o => o.Id).IsRequired().ValueGeneratedOnAdd();
            builder.Property(o => o.Key).IsRequired().HasMaxLength(50);
            builder.Property(o => o.Name).IsRequired().HasMaxLength(100);
        }
    }
}
