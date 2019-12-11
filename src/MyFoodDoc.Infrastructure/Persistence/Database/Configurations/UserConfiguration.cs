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
            builder.Property(x => x.Birthday).HasColumnType("Date");
            builder.Property(x => x.Gender).HasConversion<string>().HasMaxLength(6);
            
            builder.HasOne(x => x.Insurance).WithMany().HasForeignKey(x => x.InsuranceId);
            builder.HasMany(x => x.Indications).WithOne(x => x.User).OnDelete(DeleteBehavior.Cascade);
            builder.HasMany(x => x.Motivations).WithOne(x => x.User).OnDelete(DeleteBehavior.Cascade);
            builder.HasMany(x => x.Diets).WithOne(x => x.User).OnDelete(DeleteBehavior.Cascade);

            builder.OwnsMany(e => e.BloodSugarLevelHistory, builder =>
            {
                builder.ToTable("BloodSugarLevelHistory", "User");
                builder.WithOwner().HasForeignKey(x => x.UserId);
                builder.HasKey(x => new { x.UserId, x.Date });
                builder.Property(x => x.Date).HasColumnType("Date");
                builder.Property(x => x.Value).IsRequired();
            });

            builder.HasMany(e => e.WeightHistory).WithOne().HasForeignKey(x => x.UserId);
            builder.HasMany(e => e.AbdominalGirthHistory).WithOne().HasForeignKey(x => x.UserId);
        }
    }
}
