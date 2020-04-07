using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MyFoodDoc.Application.Entites;
using MyFoodDoc.Application.Entites.Methods;

namespace MyFoodDoc.Infrastructure.Persistence.Database.Configuration
{
    /*
    public class ChoiceMethodChoiceConfiguration : IEntityTypeConfiguration<ChoiceMethodChoice>
    {
        public void Configure(EntityTypeBuilder<ChoiceMethodChoice> builder)
        {
            builder.ToTable("ChoiceMethodChoices", "Analysis");
            builder.HasKey(o => new { o.Id, o.MethodId });
            builder.Property(p => p.Id).IsRequired().ValueGeneratedOnAdd();
            builder.Property(p => p.Text).IsRequired();
            builder.Property(p => p.IsCorrect).IsRequired();

            builder.HasOne(page => page.Method).WithMany();
        }
    }   
    */
}
