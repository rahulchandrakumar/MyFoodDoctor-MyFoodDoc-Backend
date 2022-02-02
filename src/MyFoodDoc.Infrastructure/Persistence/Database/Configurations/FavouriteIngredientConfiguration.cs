using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MyFoodDoc.Application.Entities.Diary;

namespace MyFoodDoc.Infrastructure.Persistence.Database.Configurations
{
    public class FavouriteIngredientConfiguration : IEntityTypeConfiguration<FavouriteIngredient>
    {
        public virtual void Configure(EntityTypeBuilder<FavouriteIngredient> builder)
        {
            builder.ToTable("FavouritesIngredients", "Diary");

            builder.HasKey(x => new { x.FavouriteId, x.IngredientId });
            builder.Property(p => p.Amount).IsRequired().HasColumnType("decimal(18, 2)");

            builder.HasOne(x => x.Favourite)
                .WithMany(x => x.Ingredients)
                .HasForeignKey(x => x.FavouriteId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(x => x.Ingredient)
                .WithMany(x => x.Favourites)
                .HasForeignKey(x => x.IngredientId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
