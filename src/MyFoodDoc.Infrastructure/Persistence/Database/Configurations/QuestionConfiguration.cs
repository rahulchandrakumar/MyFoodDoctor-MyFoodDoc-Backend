using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MyFoodDoc.Application.Entities.Psychogramm;

namespace MyFoodDoc.Infrastructure.Persistence.Database.Configurations
{
    public class QuestionConfiguration : IEntityTypeConfiguration<Question>
    {
        public virtual void Configure(EntityTypeBuilder<Question> builder)
        {
            builder.ToTable("Questions", "Psychogramm");

            builder.HasKey(o => o.Id);
            builder.Property(o => o.Id).IsRequired().ValueGeneratedOnAdd();
            builder.Property(o => o.Text).IsRequired().HasMaxLength(1000);
            builder.Property(o => o.Order).IsRequired();
            builder.Property(o => o.Type).HasConversion<string>().HasMaxLength(9);

            builder.HasOne(x => x.Scale).WithMany(x => x.Questions).HasForeignKey(x => x.ScaleId).OnDelete(DeleteBehavior.Cascade);
        }
    }
}
