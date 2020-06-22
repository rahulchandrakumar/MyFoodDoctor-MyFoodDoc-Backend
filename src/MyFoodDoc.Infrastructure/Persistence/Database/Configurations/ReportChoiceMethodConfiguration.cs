using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MyFoodDoc.Application.Entities;

namespace MyFoodDoc.Infrastructure.Persistence.Database.Configuration
{
    /*
    public abstract class ReportChoiceMethodConfiguration<TReportMethod> : ReportMethodConfiguration<TReportMethod> where TReportMethod : ReportMethod
    {
        public override void Configure(EntityTypeBuilder<TReportMethod> builder)
        {
            builder.HasKey(reportMethod => new { reportMethod.ReportId, reportMethod.MethodId });

            builder.Property(reportMethod => reportMethod.ReportId).IsRequired();
            builder.Property(reportMethod => reportMethod.MethodId).IsRequired();
            builder.Property(reportMethod => reportMethod.Date).IsRequired().HasColumnType("Date");
        }
    }
    */
}