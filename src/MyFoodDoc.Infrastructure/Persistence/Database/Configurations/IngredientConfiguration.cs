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
            builder.Property(o => o.Id).IsRequired().ValueGeneratedOnAdd();

            /*
            builder.HasIndex(o => new { o.ExternalKey, o.Name });
            builder.HasIndex(o => o.Amount);
            builder.Property(o => o.Name).IsRequired().HasMaxLength(100);
            builder.Property(o => o.ExternalKey).IsRequired().HasMaxLength(7).IsFixedLength();   
            */

            builder.Property(o => o.FoodId).IsRequired();
            builder.Property(o => o.FoodName).IsRequired().HasMaxLength(100);
            builder.Property(o => o.ServingId).IsRequired();
            builder.Property(o => o.ServingDescription).IsRequired().HasMaxLength(100);
            builder.Property(o => o.MetricServingAmount).IsRequired().HasColumnType("decimal(18,2)");
            builder.Property(o => o.MetricServingUnit).IsRequired().HasMaxLength(100);
            builder.Property(o => o.MeasurementDescription).IsRequired().HasMaxLength(100);
            builder.Property(o => o.LastSynchronized).IsRequired().HasColumnType("Date");
            builder.Property(o => o.Calories).HasColumnType("decimal(18,2)");
            builder.Property(o => o.Carbohydrate).HasColumnType("decimal(18,2)");
            builder.Property(o => o.Protein).HasColumnType("decimal(18,2)");
            builder.Property(o => o.Fat).HasColumnType("decimal(18,2)");
            builder.Property(o => o.SaturatedFat).HasColumnType("decimal(18,2)");
            builder.Property(o => o.PolyunsaturatedFat).HasColumnType("decimal(18,2)");
            builder.Property(o => o.MonounsaturatedFat).HasColumnType("decimal(18,2)");
            builder.Property(o => o.Cholesterol).HasColumnType("decimal(18,2)");
            builder.Property(o => o.Sodium).HasColumnType("decimal(18,2)");
            builder.Property(o => o.Potassium).HasColumnType("decimal(18,2)");
            builder.Property(o => o.Fiber).HasColumnType("decimal(18,2)");
            builder.Property(o => o.Sugar).HasColumnType("decimal(18,2)");
        }
    }
}
