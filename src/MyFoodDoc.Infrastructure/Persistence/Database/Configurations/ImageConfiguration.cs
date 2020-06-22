using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MyFoodDoc.Application.Entities;

namespace MyFoodDoc.Infrastructure.Persistence.Database.Configuration
{
    public class ImageConfiguration : IEntityTypeConfiguration<Image>
    {
        
        public void Configure(EntityTypeBuilder<Image> builder)
        {
            builder.ToTable("Images", "System");
            builder.HasKey(o => o.Id);

            // see: https://stackoverflow.com/questions/219569/best-database-field-type-for-a-url
            builder.Property(o => o.Url).IsRequired().HasMaxLength(2083);
        }
    }
}
