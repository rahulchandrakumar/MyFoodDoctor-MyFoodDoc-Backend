using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MyFoodDoc.Application.EnumEntities;
using MyFoodDoc.Infrastructure.Persistence.Database.Configurations.Abstractions;

namespace MyFoodDoc.Infrastructure.Persistence.Database.Configurations
{
    public class MotivationConfiguration : AbstractEnumConfiguration<Motivation, int>
    {
        public override void Configure(EntityTypeBuilder<Motivation> builder)
        {
            base.Configure(builder);

            builder.ToTable("Motivations", "System");
        }
    }
}
