using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MyFoodDoc.Application.Entities;
using MyFoodDoc.Infrastructure.Persistence.Database.Configurations.Abstractions;

namespace MyFoodDoc.Infrastructure.Persistence.Database.Configurations
{
    public class MealConfiguration : AbstractDiaryConfiguration<Meal, string>
    {
        public override void Configure(EntityTypeBuilder<Meal> builder)
        {
            builder.ToTable("Meals", "Diary");
            builder.HasKey(o => o.Id);

            builder.HasIndex(p => new { p.UserId, p.Date });

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
        }
    }
}
