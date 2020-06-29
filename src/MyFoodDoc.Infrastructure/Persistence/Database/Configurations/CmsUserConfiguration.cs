using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MyFoodDoc.Application.Entities;

namespace MyFoodDoc.Infrastructure.Persistence.Database.Configurations
{
    public class CmsUserConfiguration : IEntityTypeConfiguration<CmsUser>
    {
        public void Configure(EntityTypeBuilder<CmsUser> builder)
        {
            builder.ToTable("Users", "CMS");
            builder.HasKey(o => o.Id);
            builder.HasIndex(o => new { o.Username, o.Displayname });
            builder.Property(o => o.Id).IsRequired().ValueGeneratedOnAdd();
            builder.Property(o => o.Displayname).IsRequired().HasMaxLength(50);
            builder.Property(o => o.Username).IsRequired().HasMaxLength(50);
            builder.Property(o => o.PasswordHash).HasMaxLength(200);
        }
    }
}
