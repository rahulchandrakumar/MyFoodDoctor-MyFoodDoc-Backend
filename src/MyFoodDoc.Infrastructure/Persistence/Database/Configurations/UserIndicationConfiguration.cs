using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MyFoodDoc.Application.Entities;

namespace MyFoodDoc.Infrastructure.Persistence.Database.Configurations
{
    public class UserUserIndicationConfiguration : IEntityTypeConfiguration<UserIndication>
    {
        public void Configure(EntityTypeBuilder<UserIndication> builder)
        {
            builder.ToTable("UserIndications", "User");
            builder.HasKey(x => new { x.UserId, x.IndicationId });
            
            builder.HasOne(x => x.User)
                .WithMany(x => x.Indications)
                .HasForeignKey(x => x.UserId)
                .OnDelete(DeleteBehavior.Cascade);
            
            builder.HasOne(x => x.Indication)
                .WithMany()
                .HasForeignKey(x => x.IndicationId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
