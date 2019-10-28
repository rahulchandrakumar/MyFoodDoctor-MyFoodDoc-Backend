using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MyFoodDoc.Application.Entites;

namespace MyFoodDoc.Infrastructure.Persistence.Database.Configuration
{
    public class DiaryEntryConfiguration : IEntityTypeConfiguration<DiaryEntry>
    {
        public void Configure(EntityTypeBuilder<DiaryEntry> builder)
        {
            builder.ToTable("Entry", "Diary");
            builder.HasKey(o => o.Id);
            builder.Property(p => p.Id).IsRequired().ValueGeneratedOnAdd();
            builder.HasMany(o => o.Meals).WithOne();
            builder.HasOne(o => o.Liquid).WithOne();
            builder.HasOne(o => o.Exercise).WithOne();
        }
    }
}
