using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MyFoodDoc.Application.Entites;

namespace MyFoodDoc.Infrastructure.Persistence.Database.Configuration
{
    public class IngredientConfiguration : IEntityTypeConfiguration<Ingredient>
    {
        public void Configure(EntityTypeBuilder<Ingredient> builder)
        {
            builder.ToTable("Ingredients", "Food");
            builder.HasKey(o => o.Id);
            builder.HasIndex(o => new { o.ExternalKey, o.Name });
            builder.HasIndex(o => o.Amount);
            builder.Property(o => o.Id).IsRequired().ValueGeneratedOnAdd();
            builder.Property(o => o.Name).IsRequired().HasMaxLength(100);
            builder.Property(o => o.ExternalKey).IsRequired().HasMaxLength(7).IsFixedLength();            
        }
    }
}
