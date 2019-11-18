using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MyFoodDoc.Application.Entites;

namespace MyFoodDoc.Infrastructure.Persistence.Database.Configurations
{
    class LexiconEntryConfiguration : IEntityTypeConfiguration<LexiconEntry>
    {
        public void Configure(EntityTypeBuilder<LexiconEntry> builder)
        {
            builder.ToTable("Entries", "Lexicon");
            builder.HasKey(p => p.Id);
            builder.Property(p => p.Id).IsRequired().ValueGeneratedOnAdd();
            builder.Property(p => p.TitleShort).IsRequired().HasMaxLength(100);
            builder.Property(p => p.TitleLong).IsRequired().HasMaxLength(255);
            builder.Property(p => p.ImageUrl);
            builder.Property(p => p.Text).IsRequired();
        }
    }
}
