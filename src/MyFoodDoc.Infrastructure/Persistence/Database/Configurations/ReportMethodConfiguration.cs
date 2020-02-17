using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MyFoodDoc.Application.Entites;
using MyFoodDoc.Application.Entites.Abstractions;
using MyFoodDoc.Application.Enums;
using MyFoodDoc.Infrastructure.Persistence.Database.Configuration.Abstractions;
using System.Linq;

namespace MyFoodDoc.Infrastructure.Persistence.Database.Configuration
{
    public abstract class ReportMethodConfiguration<TReportMethod> : IEntityTypeConfiguration<TReportMethod> where TReportMethod : ReportMethod
    {
        public virtual void Configure(EntityTypeBuilder<TReportMethod> builder)
        {
            builder.HasKey(reportMethod => new { reportMethod.ReportId, reportMethod.MethodId });

            builder.Property(reportMethod => reportMethod.ReportId).IsRequired();
            builder.Property(reportMethod => reportMethod.MethodId).IsRequired();
            builder.Property(reportMethod => reportMethod.Date).IsRequired().HasColumnType("Date");
        }
    }
}