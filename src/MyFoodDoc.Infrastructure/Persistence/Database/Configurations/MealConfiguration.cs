using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MyFoodDoc.Application.Entites;

namespace MyFoodDoc.Infrastructure.Persistence.Database.Configurations
{
    class MealConfiguration : IEntityTypeConfiguration<Meal>
    {
        public void Configure(EntityTypeBuilder<Meal> builder)
        {
            builder.ToTable("Meals", "Diary");
            builder.HasKey(o => o.Id);
            builder.Property(p => p.Id).IsRequired().ValueGeneratedOnAdd();
            builder.Property(p => p.Mood).IsRequired();
            builder.HasMany(o => o.Ingredients);
        }
    }
}
