using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MyFoodDoc.Application.EnumEntities;
using MyFoodDoc.Infrastructure.Persistence.Database.Configuration.Abstractions;

namespace MyFoodDoc.Infrastructure.Persistence.Database.Configuration
{
    public class IndicationConfiguration : AbstractEnumConfiguration<Indication, int>
    {
        public override void Configure(EntityTypeBuilder<Indication> builder)
        {
            base.Configure(builder);

            builder.ToTable("Indications", "System");
        }
    }
}
