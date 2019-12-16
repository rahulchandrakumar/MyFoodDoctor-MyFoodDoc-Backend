using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MyFoodDoc.Application.Entites;

namespace MyFoodDoc.Infrastructure.Persistence.Database.Configuration
{
    public class UserConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.ToTable("Users", "User");
            builder.HasKey(x => x.Id);
            builder.HasIndex(x => new { x.UserName, x.Gender });
            builder.Property(x => x.Birthday).HasColumnType("Date");
            builder.Property(x => x.Gender).HasConversion<string>().HasMaxLength(6);
            
            builder.HasOne(x => x.Insurance).WithMany().HasForeignKey(x => x.InsuranceId);
            builder.HasMany(x => x.Indications).WithOne(x => x.User).OnDelete(DeleteBehavior.Cascade);
            builder.HasMany(x => x.Motivations).WithOne(x => x.User).OnDelete(DeleteBehavior.Cascade);
            builder.HasMany(x => x.Diets).WithOne(x => x.User).OnDelete(DeleteBehavior.Cascade);

            builder.HasMany(e => e.WeightHistory).WithOne(x => x.User).HasForeignKey(x => x.UserId);
            builder.HasMany(e => e.AbdominalGirthHistory).WithOne(x => x.User).HasForeignKey(x => x.UserId);
        }
    }
}
