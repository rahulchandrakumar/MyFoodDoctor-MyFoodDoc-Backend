using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MyFoodDoc.Application.Entites;

namespace MyFoodDoc.Infrastructure.Persistence.Database.Configuration
{
    public class IngredientConfiguration : IEntityTypeConfiguration<Ingredient>
    {
        private const string NutritionsDecimal = "decimal(18,2)";
        public void Configure(EntityTypeBuilder<Ingredient> builder)
        {
            builder.ToTable("Ingredients", "Food");
            builder.HasKey(o => o.Id);
            builder.Property(o => o.Id).IsRequired().ValueGeneratedOnAdd();
            builder.Property(o => o.FoodId).IsRequired();
            builder.Property(o => o.FoodName).IsRequired().HasMaxLength(100);
            builder.Property(o => o.ServingId).IsRequired();
            builder.Property(o => o.ServingDescription).IsRequired().HasMaxLength(100);
            builder.Property(o => o.MetricServingAmount).IsRequired().HasColumnType(NutritionsDecimal);
            builder.Property(o => o.MetricServingUnit).IsRequired().HasMaxLength(100);
            builder.Property(o => o.MeasurementDescription).IsRequired().HasMaxLength(100);
            builder.Property(o => o.LastSynchronized).IsRequired();
            builder.Property(o => o.Calories).HasColumnType(NutritionsDecimal);
            builder.Property(o => o.Carbohydrate).HasColumnType(NutritionsDecimal);
            builder.Property(o => o.Protein).HasColumnType(NutritionsDecimal);
            builder.Property(o => o.Fat).HasColumnType(NutritionsDecimal);
            builder.Property(o => o.SaturatedFat).HasColumnType(NutritionsDecimal);
            builder.Property(o => o.PolyunsaturatedFat).HasColumnType(NutritionsDecimal);
            builder.Property(o => o.MonounsaturatedFat).HasColumnType(NutritionsDecimal);
            builder.Property(o => o.Cholesterol).HasColumnType(NutritionsDecimal);
            builder.Property(o => o.Sodium).HasColumnType(NutritionsDecimal);
            builder.Property(o => o.Potassium).HasColumnType(NutritionsDecimal);
            builder.Property(o => o.Fiber).HasColumnType(NutritionsDecimal);
            builder.Property(o => o.Sugar).HasColumnType(NutritionsDecimal);
            builder.Property(o => o.Vegetables).HasColumnType(NutritionsDecimal);

            builder.HasIndex(i => new { i.FoodId, i.ServingId }).IsUnique();
            builder.HasIndex(i => i.LastSynchronized);
        }
    }
}
