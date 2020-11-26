using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MyFoodDoc.Application.Entities.Diary;
using MyFoodDoc.Infrastructure.Persistence.Database.Configurations.Abstractions;

namespace MyFoodDoc.Infrastructure.Persistence.Database.Configurations
{
    public class ExerciseConfiguration : AbstractDiaryConfiguration<Exercise, string>
    {
        public override void Configure(EntityTypeBuilder<Exercise> builder)
        {
            builder.ToTable("Exercises", "Diary");

            builder.HasIndex(x => new { x.UserId, x.Date });

            builder.Property(x => x.Duration).IsRequired();
            builder.Property(x => x.LastAdded).IsRequired().HasColumnType("Time");
        }
    }
}
