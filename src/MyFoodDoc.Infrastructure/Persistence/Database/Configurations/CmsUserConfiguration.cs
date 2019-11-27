using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MyFoodDoc.Application.Entites;

namespace MyFoodDoc.Infrastructure.Persistence.Database.Configuration
{
    public class CmsUserConfiguration : IEntityTypeConfiguration<CmsUser>
    {
        public void Configure(EntityTypeBuilder<CmsUser> builder)
        {
            builder.ToTable("Users", "CMS");
            builder.HasKey(p => p.Id);
            builder.Property(p => p.Id).IsRequired().ValueGeneratedOnAdd();
            builder.Property(p => p.Displayname).IsRequired().HasMaxLength(50);
            builder.Property(p => p.Username).IsRequired().HasMaxLength(25);
            builder.Property(p => p.PasswordHash).HasMaxLength(200);
        }
    }
}
