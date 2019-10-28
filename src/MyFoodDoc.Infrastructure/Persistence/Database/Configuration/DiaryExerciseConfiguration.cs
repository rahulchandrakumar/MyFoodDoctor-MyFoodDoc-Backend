using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MyFoodDoc.Application.Entites;

namespace MyFoodDoc.Infrastructure.Persistence.Database.Configuration
{
    class DiaryExerciseConfiguration : IEntityTypeConfiguration<DiaryExercise>
    {
        public void Configure(EntityTypeBuilder<DiaryExercise> builder)
        {
            builder.ToTable("Exercises", "Diary");
            builder.HasKey(o => o.Id);
            builder.Property(p => p.Id).IsRequired().ValueGeneratedOnAdd();
            builder.Property(o => o.Duration).IsRequired();
            builder.Property(o => o.Duration).IsRequired();
        }
    }
}
