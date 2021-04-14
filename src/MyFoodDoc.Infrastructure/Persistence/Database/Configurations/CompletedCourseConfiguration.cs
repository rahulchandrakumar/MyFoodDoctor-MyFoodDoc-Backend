using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MyFoodDoc.Application.Entities.Courses;

namespace MyFoodDoc.Infrastructure.Persistence.Database.Configurations
{
    public class CompletedCourseConfiguration : IEntityTypeConfiguration<CompletedCourse>
    {
        public void Configure(EntityTypeBuilder<CompletedCourse> builder)
        {
            builder.ToTable("CompletedCourses", "Course");

            builder.HasKey(x => x.UserId);

            builder.Property(x => x.UserId).IsRequired().HasMaxLength(450);

            builder.HasOne(x => x.User).WithMany().HasForeignKey(x => x.UserId).OnDelete(DeleteBehavior.Cascade);
        }
    }
}
