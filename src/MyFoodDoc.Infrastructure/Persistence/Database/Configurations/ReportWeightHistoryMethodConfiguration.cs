using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MyFoodDoc.Application.Entites;
using MyFoodDoc.Application.Entites.Methods;
using System.Linq;

namespace MyFoodDoc.Infrastructure.Persistence.Database.Configuration
{
    public class ReportWeightHistoryMethodConfiguration : ReportValueMethodConfiguration<ReportWeightHistoryMethod>
    {
        public override void Configure(EntityTypeBuilder<ReportWeightHistoryMethod> builder)
        {
            base.Configure(builder);

            builder.Property(reportMethod => reportMethod.Weight).HasColumnName("DecimalValue");
        }
    }
}
