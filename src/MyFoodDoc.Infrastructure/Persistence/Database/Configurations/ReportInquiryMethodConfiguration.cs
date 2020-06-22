using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MyFoodDoc.Application.Entities;
using MyFoodDoc.Application.Entities.Methods;
using System.Linq;

namespace MyFoodDoc.Infrastructure.Persistence.Database.Configuration
{
    /*
    public class ReportInquiryMethodConfiguration : ReportValueMethodConfiguration<ReportInquiryMethod>
    {
        public override void Configure(EntityTypeBuilder<ReportInquiryMethod> builder)
        {
            base.Configure(builder);

            builder.Property(reportMethod => reportMethod.IsConfirmed).HasColumnName("BooleanValue");
        }
    }
    */
}
