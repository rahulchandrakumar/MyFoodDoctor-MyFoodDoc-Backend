using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MyFoodDoc.Application.EnumEntities;
using MyFoodDoc.Infrastructure.Persistence.Database.Configuration.Abstractions;

namespace MyFoodDoc.Infrastructure.Persistence.Database.Configuration
{
    public class DietConfiguration : AbstractEnumConfiguration<Diet, int>
    {
        public override void Configure(EntityTypeBuilder<Diet> builder)
        {
            base.Configure(builder);

            builder.ToTable("Diets", "System");
        }
    }
}
