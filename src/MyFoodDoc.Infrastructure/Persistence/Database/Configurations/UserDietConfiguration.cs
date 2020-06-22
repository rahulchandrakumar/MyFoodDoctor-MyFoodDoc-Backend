using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MyFoodDoc.Application.Entities;

namespace MyFoodDoc.Infrastructure.Persistence.Database.Configuration
{
    public class UserDietConfiguration : IEntityTypeConfiguration<UserDiet>
    {
        public void Configure(EntityTypeBuilder<UserDiet> builder)
        {
            builder.ToTable("UserDiets", "User");
            builder.HasKey(x => new { x.UserId, x.DietId });
            
            builder.HasOne(x => x.User)
                .WithMany(x => x.Diets)
                .HasForeignKey(x => x.UserId)
                .OnDelete(DeleteBehavior.Cascade);
            builder.HasOne(x => x.Diet)
                .WithMany()
                .HasForeignKey(x => x.DietId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
