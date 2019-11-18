using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MyFoodDoc.Application.Entites;
using MyFoodDoc.Infrastructure.Persistence.Database.Configuration.Abstractions;

namespace MyFoodDoc.Infrastructure.Persistence.Database.Configuration
{
    public class ExerciseConfiguration : AbstractDiaryConfiguration<Exercise, string>
    {
        public override void Configure(EntityTypeBuilder<Exercise> builder)
        {
            builder.ToTable("Exercises", "Diary");
            builder.Property(o => o.Duration).IsRequired();
            builder.Property(o => o.LastAdded).IsRequired().HasColumnType("Time");
        }
    }
}
