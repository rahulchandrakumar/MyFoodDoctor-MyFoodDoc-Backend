using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MyFoodDoc.Application.Entities.Psychogramm;

namespace MyFoodDoc.Infrastructure.Persistence.Database.Configurations
{
    public class UserChoiceConfiguration : IEntityTypeConfiguration<UserChoice>
    {
        public virtual void Configure(EntityTypeBuilder<UserChoice> builder)
        {
            builder.ToTable("UserChoices", "Psychogramm");

            builder.HasKey(o => o.Id);
            builder.Property(o => o.Id).IsRequired().ValueGeneratedOnAdd();
            builder.Property(o => o.UserId).IsRequired().HasMaxLength(450);

            builder.HasOne(x => x.User).WithMany().HasForeignKey(x => x.UserId).OnDelete(DeleteBehavior.Cascade);
            builder.HasOne(x => x.Choice).WithMany().HasForeignKey(x => x.ChoiceId).OnDelete(DeleteBehavior.Cascade);
        }
    }
}
