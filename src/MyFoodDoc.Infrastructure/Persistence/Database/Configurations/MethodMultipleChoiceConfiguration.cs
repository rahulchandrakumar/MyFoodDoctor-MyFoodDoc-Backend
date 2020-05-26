using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MyFoodDoc.Application.Entites.Methods;

namespace MyFoodDoc.Infrastructure.Persistence.Database.Configurations
{
    public class MethodMultipleChoiceConfiguration : IEntityTypeConfiguration<MethodMultipleChoice>
    {
        public virtual void Configure(EntityTypeBuilder<MethodMultipleChoice> builder)
        {
            builder.ToTable("MethodMultipleChoice", "System");

            builder.HasKey(o => o.Id);
            builder.Property(o => o.Id).IsRequired().ValueGeneratedOnAdd();
            builder.Property(o => o.Question).IsRequired().HasMaxLength(1000);
            builder.Property(o => o.Answer).IsRequired();

            builder.HasOne(x => x.Method).WithMany().HasForeignKey(x => x.MethodId).OnDelete(DeleteBehavior.Cascade);
        }
    }
}
