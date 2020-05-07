using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MyFoodDoc.Application.Entites.Courses;
using System;
using System.Collections.Generic;
using System.Text;

namespace MyFoodDoc.Infrastructure.Persistence.Database.Configurations
{
    public class ChapterConfiguration : IEntityTypeConfiguration<Chapter>
    {
        public virtual void Configure(EntityTypeBuilder<Chapter> builder)
        {
            builder.ToTable("Chapters", "Course");

            builder.HasKey(o => o.Id);
            builder.Property(o => o.Id).IsRequired().ValueGeneratedOnAdd();
            builder.Property(o => o.Title).IsRequired().HasMaxLength(1000);
            builder.Property(o => o.Text).IsRequired().HasMaxLength(1000);
            builder.Property(o => o.Order).IsRequired();
            builder.Property(o => o.QuestionTitle).IsRequired().HasMaxLength(1000);
            builder.Property(o => o.QuestionText).IsRequired().HasMaxLength(1000);
            builder.Property(o => o.AnswerText1).IsRequired().HasMaxLength(100);
            builder.Property(o => o.AnswerText2).IsRequired().HasMaxLength(100);
            builder.Property(o => o.Answer).IsRequired();

            builder.HasOne(x => x.Course).WithMany(x => x.Chapters).HasForeignKey(x => x.CourseId).OnDelete(DeleteBehavior.Cascade);
            builder.HasOne(x => x.Image).WithMany().HasForeignKey(x => x.ImageId).OnDelete(DeleteBehavior.SetNull);
        }
    }
}
