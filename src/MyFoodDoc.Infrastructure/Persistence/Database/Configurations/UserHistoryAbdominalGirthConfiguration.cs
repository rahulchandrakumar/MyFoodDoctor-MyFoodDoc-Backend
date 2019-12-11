using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MyFoodDoc.Application.Entites;
using MyFoodDoc.Application.Entites.TrackedValus;

namespace MyFoodDoc.Infrastructure.Persistence.Database.Configuration
{
    public class UserHistoryAbdominalGirthConfiguration : IEntityTypeConfiguration<UserWeight>
    {
        public void Configure(EntityTypeBuilder<UserWeight> builder)
        {
            builder.ToTable("AbdonimalGirthHistory", "User");
            
            builder.HasKey(x => new { x.UserId, x.Date });
            builder.Property(x => x.Date).HasColumnType("Date");
            builder.Property(x => x.Value).IsRequired();
        }
    }
}
