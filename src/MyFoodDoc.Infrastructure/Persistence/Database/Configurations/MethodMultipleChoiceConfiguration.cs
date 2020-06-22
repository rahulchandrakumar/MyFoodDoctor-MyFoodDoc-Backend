using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MyFoodDoc.Application.Entities.Methods;

namespace MyFoodDoc.Infrastructure.Persistence.Database.Configurations
{
    public class MethodMultipleChoiceConfiguration : IEntityTypeConfiguration<MethodMultipleChoice>
    {
        public virtual void Configure(EntityTypeBuilder<MethodMultipleChoice> builder)
        {
            builder.ToTable("MethodMultipleChoice", "System");

            builder.HasKey(o => o.Id);
            builder.Property(o => o.Id).IsRequired().ValueGeneratedOnAdd();
            builder.Property(o => o.Title).IsRequired().HasMaxLength(100);
            builder.Property(o => o.IsCorrect).IsRequired();

            builder.HasOne(x => x.Method).WithMany().HasForeignKey(x => x.MethodId).OnDelete(DeleteBehavior.Cascade);
        }
    }
}
