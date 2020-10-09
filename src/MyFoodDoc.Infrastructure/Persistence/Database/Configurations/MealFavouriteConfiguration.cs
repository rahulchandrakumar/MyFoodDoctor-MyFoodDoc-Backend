using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MyFoodDoc.Application.Entities.Diary;

namespace MyFoodDoc.Infrastructure.Persistence.Database.Configurations
{
    public class MealFavouriteConfiguration : IEntityTypeConfiguration<MealFavourite>
    {
        public virtual void Configure(EntityTypeBuilder<MealFavourite> builder)
        {
            builder.ToTable("MealsFavourites", "Diary");

            builder.HasKey(x => new { x.MealId, x.FavouriteId });

            builder.HasOne(x => x.Meal)
                .WithMany(x => x.Favourites)
                .HasForeignKey(x => x.MealId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(x => x.Favourite)
                .WithMany(x => x.Meals)
                .HasForeignKey(x => x.FavouriteId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
