using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MyFoodDoc.Application.Entities.Aok;

namespace MyFoodDoc.Infrastructure.Persistence.Database.Configurations
{
    public class AokUserConfiguration : IEntityTypeConfiguration<AokUser>
    {
        public void Configure(EntityTypeBuilder<AokUser> builder)
        {
            builder.ToTable("AokDiets", "User");
            builder.HasKey(x => new { x.UserId });

            builder.HasOne(x => x.User)
                .WithOne()
                .HasForeignKey<AokUser>(x => x.UserId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
