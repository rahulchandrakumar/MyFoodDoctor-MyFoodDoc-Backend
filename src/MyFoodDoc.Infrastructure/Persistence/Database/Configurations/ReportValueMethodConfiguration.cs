using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MyFoodDoc.Application.Entities;
using MyFoodDoc.Application.Entities.Methods;
using System.Linq;

namespace MyFoodDoc.Infrastructure.Persistence.Database.Configuration
{
    /*
    public class ReportValueMethodConfiguration<TMethod> : ReportMethodConfiguration<TMethod> where TMethod : ReportValueMethod
    {
        public override void Configure(EntityTypeBuilder<TMethod> builder)
        {
            base.Configure(builder);

            builder.HasOne(reportMethod => reportMethod.Report).WithMany(report => report.Methods.OfType<TMethod>()).HasForeignKey(method => method.ReportId).OnDelete(DeleteBehavior.Cascade);
            builder.HasOne(reportMethod => reportMethod.Method).WithMany().HasForeignKey(reportMethod => reportMethod.MethodId).OnDelete(DeleteBehavior.Restrict);
        }
    }
    */
}
