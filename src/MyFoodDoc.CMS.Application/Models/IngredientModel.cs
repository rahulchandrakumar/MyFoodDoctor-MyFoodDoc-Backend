using System;
using MyFoodDoc.Application.Entities;

namespace MyFoodDoc.CMS.Application.Models
{
    public class IngredientModel : BaseModel<int>
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

        public decimal? Vegetables { get; set; }

        public bool ContainsPlantProtein { get; set; }

        public static IngredientModel FromEntity(Ingredient entity)
        {
            return entity == null ? null : new IngredientModel()
            {
                Id = entity.Id,
                FoodId = entity.FoodId,
                FoodName = entity.FoodName,
                ServingId = entity.ServingId,
                ServingDescription = entity.ServingDescription,
                MetricServingAmount = entity.MetricServingAmount,
                MetricServingUnit = entity.MetricServingUnit,
                MeasurementDescription = entity.MeasurementDescription,
                LastSynchronized = entity.LastSynchronized,
                Calories = entity.Calories,
                Carbohydrate = entity.Carbohydrate,
                Protein = entity.Protein,
                Fat = entity.Fat,
                SaturatedFat = entity.SaturatedFat,
                PolyunsaturatedFat = entity.PolyunsaturatedFat,
                MonounsaturatedFat = entity.MonounsaturatedFat,
                Cholesterol = entity.Cholesterol,
                Sodium = entity.Sodium,
                Potassium = entity.Potassium,
                Fiber = entity.Fiber,
                Sugar = entity.Sugar,
                Vegetables = entity.Vegetables,
                ContainsPlantProtein = entity.ContainsPlantProtein
            };
        }

        public Ingredient ToEntity()
        {
            return new Ingredient()
            {
                Id = this.Id,
                FoodId = this.FoodId,
                FoodName = this.FoodName,
                ServingId = this.ServingId,
                ServingDescription = this.ServingDescription,
                MetricServingAmount = this.MetricServingAmount,
                MetricServingUnit = this.MetricServingUnit,
                MeasurementDescription = this.MeasurementDescription,
                LastSynchronized = this.LastSynchronized,
                Calories = this.Calories,
                Carbohydrate = this.Carbohydrate,
                Protein = this.Protein,
                Fat = this.Fat,
                SaturatedFat = this.SaturatedFat,
                PolyunsaturatedFat = this.PolyunsaturatedFat,
                MonounsaturatedFat = this.MonounsaturatedFat,
                Cholesterol = this.Cholesterol,
                Sodium = this.Sodium,
                Potassium = this.Potassium,
                Fiber = this.Fiber,
                Sugar = this.Sugar,
                Vegetables = this.Vegetables,
                ContainsPlantProtein = this.ContainsPlantProtein
            };
        }
    }
}
