using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MyFoodDoc.Application.Entities.Courses;

namespace MyFoodDoc.Infrastructure.Persistence.Database.Configurations
{
    public class SubchapterConfiguration : IEntityTypeConfiguration<Subchapter>
    {
        public virtual void Configure(EntityTypeBuilder<Subchapter> builder)
        {
            builder.ToTable("Subchapters", "Course");

            builder.HasKey(o => o.Id);
            builder.Property(o => o.Id).IsRequired().ValueGeneratedOnAdd();
            builder.Property(o => o.Title).IsRequired().HasMaxLength(1000);
            builder.Property(o => o.Text).IsRequired().HasMaxLength(1000);
            builder.Property(o => o.Order).IsRequired();

            builder.HasOne(x => x.Chapter).WithMany(x => x.Subchapters).HasForeignKey(x => x.ChapterId).OnDelete(DeleteBehavior.Cascade);
        }
    }
}
