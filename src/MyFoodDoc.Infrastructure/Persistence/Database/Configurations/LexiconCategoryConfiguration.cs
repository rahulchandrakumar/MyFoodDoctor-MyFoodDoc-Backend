using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MyFoodDoc.Application.Entities;

namespace MyFoodDoc.Infrastructure.Persistence.Database.Configurations
{
    public class LexiconCategoryConfiguration : IEntityTypeConfiguration<LexiconCategory>
    {
        public void Configure(EntityTypeBuilder<LexiconCategory> builder)
        {
            builder.ToTable("Categories", "Lexicon");

            builder.HasKey(p => p.Id);
            builder.Property(p => p.Id).IsRequired().ValueGeneratedOnAdd();
            builder.Property(p => p.Title).IsRequired().HasMaxLength(100);
            builder.Property(p => p.ImageId).IsRequired();

            builder.HasOne(x => x.Image).WithMany().HasForeignKey(x => x.ImageId);
        }
    }
}
