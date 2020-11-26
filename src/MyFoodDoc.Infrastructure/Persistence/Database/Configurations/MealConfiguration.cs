using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MyFoodDoc.Application.Entities.Diary;
using MyFoodDoc.Infrastructure.Persistence.Database.Configurations.Abstractions;

namespace MyFoodDoc.Infrastructure.Persistence.Database.Configurations
{
    public class MealConfiguration : AbstractDiaryConfiguration<Meal, string>
    {
        public override void Configure(EntityTypeBuilder<Meal> builder)
        {
            builder.ToTable("Meals", "Diary");

            builder.HasIndex(x => new { x.UserId, x.Date });

            builder.Property(x => x.Time)
                .IsRequired()
                .HasColumnType("Time");

            builder.Property(x => x.Type)
                .HasConversion<string>()
                .HasMaxLength(10);

            builder.Property(x => x.Mood);
        }
    }
}
