using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MyFoodDoc.Application.Entites;

namespace MyFoodDoc.Infrastructure.Persistence.Database.Configuration
{
    public class UserUserMotivationConfiguration : IEntityTypeConfiguration<UserMotivation>
    {
        public void Configure(EntityTypeBuilder<UserMotivation> builder)
        {
            builder.ToTable("UserMotivations", "User");
            builder.HasKey(x => new { x.UserId, x.MotivationId });
            
            builder.HasOne(x => x.User)
                .WithMany(x => x.Motivations)
                .HasForeignKey(x => x.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(x => x.Motivation)
                .WithMany()
                .HasForeignKey(x => x.MotivationId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
