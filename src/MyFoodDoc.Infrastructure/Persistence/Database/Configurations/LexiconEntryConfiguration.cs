using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MyFoodDoc.Application.Entities;

namespace MyFoodDoc.Infrastructure.Persistence.Database.Configurations
{
    public class LexiconEntryConfiguration : IEntityTypeConfiguration<LexiconEntry>
    {
        public void Configure(EntityTypeBuilder<LexiconEntry> builder)
        {
            builder.ToTable("Entries", "Lexicon");
            builder.HasKey(p => p.Id);
            builder.HasIndex(p => new { p.TitleShort, p.TitleLong });
            builder.Property(p => p.Id).IsRequired().ValueGeneratedOnAdd();
            builder.Property(p => p.TitleShort).IsRequired().HasMaxLength(30);
            builder.Property(p => p.TitleLong).IsRequired().HasMaxLength(200);
            builder.Property(p => p.ImageId).IsRequired();
            builder.Property(p => p.Text).IsRequired();

            builder.HasOne(x => x.Category).WithMany(x => x.Entries).HasForeignKey(x => x.CategoryId).OnDelete(DeleteBehavior.Cascade);
            builder.HasOne(x => x.Image).WithMany().HasForeignKey(x => x.ImageId);
        }
    }
}
