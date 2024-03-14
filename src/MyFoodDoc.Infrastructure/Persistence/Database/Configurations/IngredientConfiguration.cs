using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MyFoodDoc.Application.Entities;

namespace MyFoodDoc.Infrastructure.Persistence.Database.Configurations
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
            builder.Property(o => o.FoodName).IsRequired().HasMaxLength(500);
            builder.Property(o => o.ServingId).IsRequired();
            builder.Property(o => o.ServingDescription).IsRequired().HasMaxLength(100);
            builder.Property(o => o.MetricServingAmount).IsRequired().HasColumnType(NutritionsDecimal);
            builder.Property(o => o.MetricServingUnit).HasMaxLength(100);
            builder.Property(o => o.MeasurementDescription).IsRequired().HasMaxLength(100);
            builder.Property(o => o.LastSynchronized).IsRequired();
            builder.Property(o => o.Calories).HasColumnType(NutritionsDecimal);
            builder.Property(o => o.CaloriesExternal).IsRequired().HasColumnType(NutritionsDecimal).HasDefaultValue(0);
            builder.Property(o => o.Carbohydrate).HasColumnType(NutritionsDecimal);
            builder.Property(o => o.CarbohydrateExternal).IsRequired().HasColumnType(NutritionsDecimal).HasDefaultValue(0);
            builder.Property(o => o.Protein).HasColumnType(NutritionsDecimal);
            builder.Property(o => o.ProteinExternal).IsRequired().HasColumnType(NutritionsDecimal).HasDefaultValue(0);
            builder.Property(o => o.Fat).HasColumnType(NutritionsDecimal);
            builder.Property(o => o.FatExternal).IsRequired().HasColumnType(NutritionsDecimal).HasDefaultValue(0);
            builder.Property(o => o.SaturatedFat).HasColumnType(NutritionsDecimal);
            builder.Property(o => o.SaturatedFatExternal).IsRequired().HasColumnType(NutritionsDecimal).HasDefaultValue(0);
            builder.Property(o => o.PolyunsaturatedFat).HasColumnType(NutritionsDecimal);
            builder.Property(o => o.PolyunsaturatedFatExternal).IsRequired().HasColumnType(NutritionsDecimal).HasDefaultValue(0);
            builder.Property(o => o.MonounsaturatedFat).HasColumnType(NutritionsDecimal);
            builder.Property(o => o.MonounsaturatedFatExternal).IsRequired().HasColumnType(NutritionsDecimal).HasDefaultValue(0);
            builder.Property(o => o.Cholesterol).HasColumnType(NutritionsDecimal);
            builder.Property(o => o.CholesterolExternal).IsRequired().HasColumnType(NutritionsDecimal).HasDefaultValue(0);
            builder.Property(o => o.Sodium).HasColumnType(NutritionsDecimal);
            builder.Property(o => o.SodiumExternal).IsRequired().HasColumnType(NutritionsDecimal).HasDefaultValue(0);
            builder.Property(o => o.Potassium).HasColumnType(NutritionsDecimal);
            builder.Property(o => o.PotassiumExternal).IsRequired().HasColumnType(NutritionsDecimal).HasDefaultValue(0);
            builder.Property(o => o.Fiber).HasColumnType(NutritionsDecimal);
            builder.Property(o => o.FiberExternal).IsRequired().HasColumnType(NutritionsDecimal).HasDefaultValue(0);
            builder.Property(o => o.Sugar).HasColumnType(NutritionsDecimal);
            builder.Property(o => o.SugarExternal).IsRequired().HasColumnType(NutritionsDecimal).HasDefaultValue(0);
            builder.Property(o => o.Vegetables).HasColumnType(NutritionsDecimal);
            builder.Property(x => x.ContainsPlantProtein).HasDefaultValue(false);

            builder.HasIndex(i => new { i.FoodId, i.ServingId }).IsUnique();
            builder.HasIndex(i => i.LastSynchronized);
        }
    }
}
