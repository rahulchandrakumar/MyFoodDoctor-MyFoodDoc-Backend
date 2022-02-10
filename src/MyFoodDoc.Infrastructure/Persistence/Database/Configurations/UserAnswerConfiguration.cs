using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MyFoodDoc.Application.Entities.Courses;

namespace MyFoodDoc.Infrastructure.Persistence.Database.Configurations
{
    public class UserAnswerConfiguration : IEntityTypeConfiguration<UserAnswer>
    {
        public virtual void Configure(EntityTypeBuilder<UserAnswer> builder)
        {
            builder.ToTable("UserAnswers", "Course");

            builder.HasKey(o => o.Id);
            builder.Property(o => o.Id).IsRequired().ValueGeneratedOnAdd();
            builder.Property(o => o.UserId).IsRequired().HasMaxLength(450);
            builder.Property(o => o.Answer).IsRequired();

            builder.HasOne(x => x.User).WithMany().HasForeignKey(x => x.UserId).OnDelete(DeleteBehavior.Cascade);
            builder.HasOne(x => x.Chapter).WithMany().HasForeignKey(x => x.ChapterId).OnDelete(DeleteBehavior.Cascade);
        }
    }
}
