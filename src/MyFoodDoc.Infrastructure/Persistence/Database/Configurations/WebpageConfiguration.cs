using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MyFoodDoc.Application.Entities;

namespace MyFoodDoc.Infrastructure.Persistence.Database.Configuration
{
    public class WebPageConfiguration : IEntityTypeConfiguration<WebPage>
    {
        public void Configure(EntityTypeBuilder<WebPage> builder)
        {
            builder.ToTable("WebPages", "System");
            builder.HasKey(o => o.Id);
            builder.HasIndex(o => o.Title);
            builder.Property(o => o.Url).HasMaxLength(2083);
            builder.Property(o => o.Title).IsRequired().HasMaxLength(200);
            builder.Property(o => o.Text).IsRequired();
            builder.Property(o => o.IsDeletable).IsRequired();
        }
    }
}
