using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MyFoodDoc.Application.Entites;

namespace MyFoodDoc.Infrastructure.Persistence.Database.Configuration
{
    /*
    public class ReportChoiceMethodChoiceConfiguration : IEntityTypeConfiguration<ReportChoiceMethodChoice>
    {
        public void Configure(EntityTypeBuilder<ReportChoiceMethodChoice> builder)
        {
            builder.ToTable("ReportChoiceMethodChoices", "Analysis");

            builder.HasOne(o => o.ReportMethod).WithMany().HasForeignKey(o => new { o.ReportId, o.MethodId }).OnDelete(DeleteBehavior.Cascade);
            builder.HasOne(o => o.Choice).WithMany().HasForeignKey(o => new { o.ChoiceId, o.MethodId }).OnDelete(DeleteBehavior.Restrict);
        }
    }   
    */
}
