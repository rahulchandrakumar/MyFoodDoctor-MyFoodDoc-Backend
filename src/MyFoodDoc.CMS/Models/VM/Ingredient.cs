using MyFoodDoc.CMS.Application.Models;
using System;

namespace MyFoodDoc.CMS.Models.VM
{
    public class Ingredient : VMBase.BaseModel<int>
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

        public static Ingredient FromModel(IngredientModel model)
        {
            return model == null ? null : new Ingredient()
            {
                Id = model.Id,
                FoodId = model.FoodId,
                FoodName = model.FoodName,
                ServingId = model.ServingId,
                ServingDescription = model.ServingDescription,
                MetricServingAmount = model.MetricServingAmount,
                MetricServingUnit = model.MetricServingUnit,
                MeasurementDescription = model.MeasurementDescription,
                LastSynchronized = model.LastSynchronized,
                Calories = model.Calories,
                Carbohydrate = model.Carbohydrate,
                Protein = model.Protein,
                Fat = model.Fat,
                SaturatedFat = model.SaturatedFat,
                PolyunsaturatedFat = model.PolyunsaturatedFat,
                MonounsaturatedFat = model.MonounsaturatedFat,
                Cholesterol = model.Cholesterol,
                Sodium = model.Sodium,
                Potassium = model.Potassium,
                Fiber = model.Fiber,
                Sugar = model.Sugar,
                Vegetables = model.Vegetables,
                ContainsPlantProtein = model.ContainsPlantProtein
            };
        }

        public IngredientModel ToModel()
        {
            return new IngredientModel()
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
