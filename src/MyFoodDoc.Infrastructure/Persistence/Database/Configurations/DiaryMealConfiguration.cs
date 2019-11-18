using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MyFoodDoc.Application.Entites;

namespace MyFoodDoc.Infrastructure.Persistence.Database.Configurations
{
    class DiaryMealConfiguration : IEntityTypeConfiguration<DiaryMeal>
    {
        public void Configure(EntityTypeBuilder<DiaryMeal> builder)
        {
            builder.ToTable("Meals", "Diary");
            builder.HasKey(o => o.Id);
            builder.Property(p => p.Id).IsRequired().ValueGeneratedOnAdd();
            builder.Property(p => p.Mood).IsRequired();
            builder.HasMany(o => o.Ingredients);
        }
    }
}
