using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MyFoodDoc.Application.Entities;
using MyFoodDoc.Infrastructure.Persistence.Database.Configurations.Abstractions;

namespace MyFoodDoc.Infrastructure.Persistence.Database.Configurations
{
    public class LiquidConfiguration : AbstractDiaryConfiguration<Liquid, string>
    {
        public override void Configure(EntityTypeBuilder<Liquid> builder)
        {
            builder.ToTable("Liquids", "Diary");
            builder.Property(o => o.Amount).IsRequired();
            builder.Property(o => o.LastAdded).IsRequired().HasColumnType("Time");
        }
    }
}
