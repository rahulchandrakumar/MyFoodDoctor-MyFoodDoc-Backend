using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MyFoodDoc.Application.Entities.Diary;
using MyFoodDoc.Infrastructure.Persistence.Database.Configurations.Abstractions;

namespace MyFoodDoc.Infrastructure.Persistence.Database.Configurations
{
    public class LiquidConfiguration : AbstractDiaryConfiguration<Liquid, string>
    {
        public override void Configure(EntityTypeBuilder<Liquid> builder)
        {
            builder.ToTable("Liquids", "Diary");

            builder.HasIndex(x => new { x.UserId, x.Date });

            builder.Property(x => x.Amount).IsRequired();
            builder.Property(x => x.LastAdded).IsRequired().HasColumnType("Time");
        }
    }
}
