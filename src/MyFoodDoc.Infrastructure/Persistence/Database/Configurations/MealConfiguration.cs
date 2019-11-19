using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MyFoodDoc.Application.Entites;
using MyFoodDoc.Application.Enums;
using MyFoodDoc.Infrastructure.Persistence.Database.Configuration.Abstractions;

namespace MyFoodDoc.Infrastructure.Persistence.Database.Configuration
{
    public class MealConfiguration : AbstractDiaryConfiguration<Meal, string>
    {
        public override void Configure(EntityTypeBuilder<Meal> builder)
        {
            builder.ToTable("Meals", "Diary");
            builder.HasKey(o => o.Id);

            builder.HasIndex(p => new { p.Date, p.Type })
                .IsUnique()
                .HasFilter($"Type = '{nameof(MealType.Breakfast)}' or Type = '{nameof(MealType.Lunch)}' or Type = '{nameof(MealType.Dinner)}'");

            builder.Property(p => p.Id)
                .IsRequired()
                .ValueGeneratedOnAdd();

            builder.Property(p => p.Time)
                .IsRequired()
                .HasColumnType("Time");

            builder.Property(x => x.Type)
                .HasConversion<string>()
                .HasMaxLength(10);

            builder.Property(p => p.Mood);

            builder.HasMany(o => o.Ingredients).WithOne();
        }
    }
}
