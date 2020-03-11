using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MyFoodDoc.Application.Entites;
using MyFoodDoc.Infrastructure.Persistence.Database.Configuration.Abstractions;

namespace MyFoodDoc.Infrastructure.Persistence.Database.Configuration
{
    public class MealIngredientConfiguration : IEntityTypeConfiguration<MealIngredient>
    {
        public void Configure(EntityTypeBuilder<MealIngredient> builder)
        {
            builder.ToTable("MealsIngredients", "Diary");
            builder.HasKey(x => new { x.MealId, x.IngredientId });
            builder.Property(p => p.Amount).IsRequired().HasColumnType("decimal(18, 2)");

            builder.HasOne(x => x.Meal)
                .WithMany(x => x.Ingredients)
                .HasForeignKey(x => x.MealId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(x => x.Ingredient)
                .WithMany(x => x.Meals)
                .HasForeignKey(x => x.IngredientId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
