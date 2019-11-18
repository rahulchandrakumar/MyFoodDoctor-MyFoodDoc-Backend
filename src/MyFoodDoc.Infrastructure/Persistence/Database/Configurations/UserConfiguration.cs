using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MyFoodDoc.Application.Entites;

namespace MyFoodDoc.Infrastructure.Persistence.Database.Configurations
{
    class UserConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.ToTable("Profiles", "User");
            builder.HasKey(x => x.Id);
            builder.Property(p => p.Id).IsRequired().ValueGeneratedOnAdd();
            builder.Property(x => x.Birthday).HasMaxLength(10);
            builder.Property(x => x.Gender).HasConversion<string>();
            builder.HasOne(x => x.Insurance).WithMany().IsRequired();
            builder.HasMany(c => c.Motivations).WithOne();
            builder.HasMany(c => c.Indications).WithOne();
        }
    }
}
