using System;
using MyFoodDoc.Application.Abstractions;
using System.Collections.Generic;

namespace MyFoodDoc.Application.Entites
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

        public decimal? Carbohydrate { get; set; }

        public decimal? Protein { get; set; }

        public decimal? Fat { get; set; }

        public decimal? SaturatedFat { get; set; }

        public decimal? PolyunsaturatedFat { get; set; }

        public decimal? MonounsaturatedFat { get; set; }

        public decimal? Cholesterol { get; set; }

        public decimal? Sodium { get; set; }

        public decimal? Potassium { get; set; }

        public decimal? Fiber { get; set; }

        public decimal? Sugar { get; set; }
        
        public ICollection<MealIngredient> Meals { get; set; }
    }
}
