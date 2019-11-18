using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MyFoodDoc.Application.Entites;

namespace MyFoodDoc.Infrastructure.Persistence.Database.Configurations
{
    class DiaryMealsIngredientsConfiguration : IEntityTypeConfiguration<Meal>
    {
        public void Configure(EntityTypeBuilder<Meal> builder)
        {
            builder.ToTable("MealsIngredients", "Diary");
        }
    }
}
