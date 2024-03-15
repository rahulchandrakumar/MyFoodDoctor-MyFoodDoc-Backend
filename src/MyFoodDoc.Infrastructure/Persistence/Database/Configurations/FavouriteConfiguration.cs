using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MyFoodDoc.Application.Entities.Diary;

namespace MyFoodDoc.Infrastructure.Persistence.Database.Configurations
{
    public class FavouriteConfiguration : IEntityTypeConfiguration<Favourite>
    {
        public virtual void Configure(EntityTypeBuilder<Favourite> builder)
        {
            builder.ToTable("Favourites", "Diary");

            builder.HasKey(o => o.Id);
            builder.Property(o => o.UserId).IsRequired().HasMaxLength(450);
            builder.Property(o => o.Title).IsRequired().HasMaxLength(500);

            builder.HasOne(x => x.User).WithMany(x => x.Favourites).HasForeignKey(x => x.UserId).OnDelete(DeleteBehavior.Cascade);
        }
    }
}
