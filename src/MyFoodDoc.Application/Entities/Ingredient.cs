using System;
using MyFoodDoc.Application.Abstractions;
using System.Collections.Generic;

namespace MyFoodDoc.Application.Entities
{
    public class Ingredient : AbstractAuditableEntity
    {
        public long FoodId { get; set; }

        public string FoodName { get; set; }

        public long ServingId { get; set; }

        public string ServingDescription { get; set; }

        public decimal MetricServingAmount { get; set; }

        public string MetricServingUnit { get; set; }

        public string MeasurementDescription { get; set; }

        public DateTime LastSynchronized { get; set; }

        public decimal? Calories { get; set; }

        public decimal CaloriesExternal { get; set; }

        public decimal? Carbohydrate { get; set; }

        public decimal CarbohydrateExternal { get; set; }

        public decimal? Protein { get; set; }

        public decimal ProteinExternal { get; set; }

        public decimal? Fat { get; set; }

        public decimal FatExternal { get; set; }

        public decimal? SaturatedFat { get; set; }

        public decimal SaturatedFatExternal { get; set; }

        public decimal? PolyunsaturatedFat { get; set; }

        public decimal PolyunsaturatedFatExternal { get; set; }

        public decimal? MonounsaturatedFat { get; set; }

        public decimal MonounsaturatedFatExternal { get; set; }

        public decimal? Cholesterol { get; set; }

        public decimal CholesterolExternal { get; set; }

        public decimal? Sodium { get; set; }

        public decimal SodiumExternal { get; set; }

        public decimal? Potassium { get; set; }

        public decimal PotassiumExternal { get; set; }

        public decimal? Fiber { get; set; }

        public decimal FiberExternal { get; set; }

        public decimal? Sugar { get; set; }

        public decimal SugarExternal { get; set; }

        public decimal? Vegetables { get; set; }

        public bool ContainsPlantProtein { get; set; }

        public ICollection<MealIngredient> Meals { get; set; }
    }
}
