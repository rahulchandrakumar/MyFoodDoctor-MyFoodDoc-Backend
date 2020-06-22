using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MyFoodDoc.Application.Entities;
using MyFoodDoc.Application.Entities.Methods;
using MyFoodDoc.Infrastructure.Persistence.Database.Configuration;
using System.Linq;

namespace MyFoodDoc.Infrastructure.Persistence.Database.Configurations
{
    /*
    public class ReportMoodMethodConfiguration : ReportValueMethodConfiguration<ReportMoodMethod>
    {
        public override void Configure(EntityTypeBuilder<ReportMoodMethod> builder)
        {
            base.Configure(builder);

            builder.Property(reportMethod => reportMethod.Mood).HasColumnName("IntValue");
        }
    }
    */
}
