using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MyFoodDoc.Application.Entites;
using MyFoodDoc.Application.Entites.Methods;
using System.Linq;

namespace MyFoodDoc.Infrastructure.Persistence.Database.Configuration
{
    public class ReportSuggestionMethodConfiguration : ReportValueMethodConfiguration<ReportSuggestionMethod>
    {
        public override void Configure(EntityTypeBuilder<ReportSuggestionMethod> builder)
        {
            base.Configure(builder);

            builder.Property(reportMethod => reportMethod.IsAccepted).HasColumnName("BooleanValue");
        }
    }
}
