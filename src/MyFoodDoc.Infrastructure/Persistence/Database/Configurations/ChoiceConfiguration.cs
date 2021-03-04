using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MyFoodDoc.Application.Entities.Psychogramm;

namespace MyFoodDoc.Infrastructure.Persistence.Database.Configurations
{
    public class ChoiceConfiguration : IEntityTypeConfiguration<Choice>
    {
        public virtual void Configure(EntityTypeBuilder<Choice> builder)
        {
            builder.ToTable("Choices", "Psychogramm");

            builder.HasKey(o => o.Id);
            builder.Property(o => o.Id).IsRequired().ValueGeneratedOnAdd();
            builder.Property(o => o.Text).IsRequired().HasMaxLength(1000);
            builder.Property(o => o.Order).IsRequired();

            builder.HasOne(x => x.Question).WithMany(x => x.Choices).HasForeignKey(x => x.QuestionId).OnDelete(DeleteBehavior.Cascade);
        }
    }
}
