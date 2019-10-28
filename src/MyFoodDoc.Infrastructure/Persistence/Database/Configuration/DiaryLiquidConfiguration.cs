using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MyFoodDoc.Application.Entites;

namespace MyFoodDoc.Infrastructure.Persistence.Database.Configuration
{
    class DiaryLiquidConfiguration : IEntityTypeConfiguration<DiaryLiquid>
    {
        public void Configure(EntityTypeBuilder<DiaryLiquid> builder)
        {
            builder.ToTable("Liquids", "Diary");
            builder.HasKey(o => o.Id);
            builder.Property(p => p.Id).IsRequired().ValueGeneratedOnAdd();
        }
    }
}
