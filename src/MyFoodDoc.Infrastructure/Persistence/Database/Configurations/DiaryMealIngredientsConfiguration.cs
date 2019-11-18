using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MyFoodDoc.Application.Entites;

namespace MyFoodDoc.Infrastructure.Persistence.Database.Configurations
{
    class DiaryMealsIngredientsConfiguration : IEntityTypeConfiguration<DiaryMeal>
    {
        public void Configure(EntityTypeBuilder<DiaryMeal> builder)
        {
            builder.ToTable("MealsIngredients", "Diary");
        }
    }
}
