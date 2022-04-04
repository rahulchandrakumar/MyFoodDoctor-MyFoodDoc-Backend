using MyFoodDoc.Application.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MyFoodDoc.Application.Entities.Html;

namespace MyFoodDoc.Application.Services
{
    public static class HtmlParser
    {
        public static string ParseHtml(DiaryExportModel data, HtmlStructure htmlStruct)
        {
            var sp = new StringBuilder(1204);
            sp.Append(htmlStruct.Header);
            sp.Append(PrepareHeader(data.DateFrom, data.DateTo, htmlStruct.Main));
            sp.Append(htmlStruct.TableHeader);
            sp.Append(PrepareContent(data.Days, htmlStruct));
            sp.Append(htmlStruct.Footer);
            return sp.ToString();
        }

        private static string PrepareContent(ICollection<DiaryExportDayModel> days, HtmlStructure htmlStruct)
        {
            var sp = new StringBuilder(1204);

            foreach (var day in days)
            {
                var content = PrepareContent(day, htmlStruct);
                sp.Append(content);

            }
            return sp.ToString();
        }

        private static string PrepareContent(DiaryExportDayModel day, HtmlStructure htmlStruct)
        {
            var total = htmlStruct.Total.Replace(HtmlToken.Calories, day.Calories.ToString())
                                        .Replace(HtmlToken.Vegetables, day.Vegetables.ToString())
                                        .Replace(HtmlToken.Protein, day.Protein.ToString())
                                        .Replace(HtmlToken.Sugar, day.Sugar.ToString())
                                        .Replace(HtmlToken.Count, day.Meals.Count.ToString())
                                        .Replace(HtmlToken.LiquidAmount, day.LiquidAmount.ToString())
                                        .Replace(HtmlToken.ExerciseDuration, day.ExerciseDuration.ToString());

            return htmlStruct.Content
                .Replace(HtmlToken.Time, day.Date.ToString("dd.MM"))
                .Replace(HtmlToken.Meals, GetMeals(day.Meals, htmlStruct))
                .Replace(HtmlToken.Total, total);
        }

        private static string GetMeals(ICollection<DiaryExportMealModel> meals, HtmlStructure htmlStruct)
        {
            var breakfastStringBuilder = new StringBuilder(1024);
            var lunchStringBuilder = new StringBuilder(1024);
            var dinerStringBuilder = new StringBuilder(1204);
            var snackStringBuilder = new StringBuilder(1204);

            foreach (var meal in meals)
            {
                var time = htmlStruct.MealStructure.Time.Replace(HtmlToken.Time, meal.Time.ToString(@"hh\:mm"));
                var ingredients = meal.Ingredients.Select(i => htmlStruct.MealStructure.Ingredient.Replace(HtmlToken.IngredientName, i.FoodName)
                                                                                                         .Replace(HtmlToken.ServingDescription, i.ServingDescription));
                var stringIngrediens = htmlStruct.MealStructure.Ingredients.Replace(HtmlToken.Ingredients, String.Join("", ingredients));
                var mealContent = htmlStruct.MealStructure.Content.Replace(HtmlToken.Time, time).Replace(HtmlToken.Content, stringIngrediens);
                switch (meal.Type)
                {
                    case Enums.MealType.Breakfast:
                        breakfastStringBuilder.Append(mealContent);
                        break;
                    case Enums.MealType.Lunch:
                        lunchStringBuilder.Append(mealContent);
                        break;
                    case Enums.MealType.Dinner:
                        dinerStringBuilder.Append(mealContent);
                        break;
                    case Enums.MealType.Snack:
                        snackStringBuilder.Append(mealContent);
                        break;
                    default:
                        break;
                }
            }

            var breakfast = htmlStruct.MealTemplate.Replace(HtmlToken.Meal, breakfastStringBuilder.ToString());
            var lunch = htmlStruct.MealTemplate.Replace(HtmlToken.Meal, lunchStringBuilder.ToString());
            var diner = htmlStruct.MealTemplate.Replace(HtmlToken.Meal, dinerStringBuilder.ToString());
            var snack = htmlStruct.MealTemplate.Replace(HtmlToken.Meal, snackStringBuilder.ToString());
            return htmlStruct.Meals
                .Replace(HtmlToken.Breakfast, breakfast)
                .Replace(HtmlToken.Lunch, lunch)
                .Replace(HtmlToken.Diner, diner)
                .Replace(HtmlToken.Snack, snack);
        }

        private static string PrepareHeader(DateTime dateFrom, DateTime dateTo, string mainStruct)
        {
            return mainStruct
                .Replace(HtmlToken.DateFrom, dateFrom.ToString("dd.MM.yyyy"))
                .Replace(HtmlToken.DateTo, dateTo.ToString("dd.MM.yyyy"));
        }
    }
}
