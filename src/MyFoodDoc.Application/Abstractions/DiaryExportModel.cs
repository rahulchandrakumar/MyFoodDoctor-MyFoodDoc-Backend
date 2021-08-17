using MyFoodDoc.Application.Enums;
using System;
using System.Collections.Generic;

namespace MyFoodDoc.Application.Abstractions
{
    public class DiaryExportModel
    {
        public DateTime DateFrom { get; set; }
        public DateTime DateTo { get; set; }

        public ICollection<DiaryExportDayModel> Days { get; set; }
    }

    public class DiaryExportDayModel
    {
        public DateTime Date { get; set; }

        public decimal Calories { get; set; }

        public decimal Vegetables { get; set; }

        public decimal Protein { get; set; }

        public decimal Sugar { get; set; }

        public int LiquidAmount { get; set; }

        public int ExerciseDuration { get; set; }

        public ICollection<DiaryExportMealModel> Meals { get; set; }
    }

    public class DiaryExportMealModel
    {
        public TimeSpan Time { get; set; }

        public MealType Type { get; set; }

        public ICollection<DiaryExportMealIngredientModel> Ingredients { get; set; }
    }

    public class DiaryExportMealIngredientModel
    {
        public string FoodName { get; set; }

        public string ServingDescription { get; set; }

        public string MeasurementDescription { get; set; }

        public decimal MetricServingAmount { get; set; }

        public string MetricServingUnit { get; set; }

        public decimal Amount { get; set; }
    }
}
