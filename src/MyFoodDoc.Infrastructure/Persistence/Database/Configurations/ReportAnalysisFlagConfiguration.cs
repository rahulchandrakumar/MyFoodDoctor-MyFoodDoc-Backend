using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MyFoodDoc.Application.Entites;
using MyFoodDoc.Application.Enums;
using MyFoodDoc.Infrastructure.Persistence.Database.Configuration.Abstractions;

namespace MyFoodDoc.Infrastructure.Persistence.Database.Configuration
{
    /*
    public class ReportAnalysisFlagConfiguration : IEntityTypeConfiguration<ReportAnalysisFlag>
    {
        public void Configure(EntityTypeBuilder<ReportAnalysisFlag> builder)
        {
            builder.ToTable("ReportAnalysisFlags", "Analysis");
            builder.HasKey(o => new { o.ReportId, o.AnalysisFlagId });

            builder.HasOne(o => o.Report)
                .WithMany(o => o.Flags)
                .HasForeignKey(o => o.ReportId);

            builder.HasOne(o => o.Flag)
                .WithMany()
                .HasForeignKey(o => o.AnalysisFlagId);
        }
    }
    */
}
