using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace MyFoodDoc.App.Application.Clients.FatSecret
{
    public class GetFoodResult
    {
        [JsonProperty("food")]
        public Food Food { get; set; }
    }

    public class Food
    {
        [JsonProperty("food_id")]
        public long Id { get; set; }

        [JsonProperty("food_name")]
        public string Name { get; set; }

        [JsonProperty("food_type")]
        public string Type { get; set; }

        [JsonProperty("food_url")]
        public string Url { get; set; }

        [JsonProperty("servings")]
        public Servings Servings { get; set; }
    }

    public class Servings
    {
        [JsonProperty("serving")]
        public Serving[] Serving { get; set; }
    }

    public class Serving
    {
        [JsonProperty("calcium")]
        public decimal Calcium { get; set; }

        [JsonProperty("calories")]
        public decimal Calories { get; set; }

        [JsonProperty("carbohydrate")]
        public decimal Carbohydrate { get; set; }

        [JsonProperty("cholesterol")]
        public decimal Cholesterol { get; set; }

        [JsonProperty("fat")]
        public decimal Fat { get; set; }

        [JsonProperty("fiber")]
        public decimal Fiber { get; set; }

        [JsonProperty("iron")]
        public decimal Iron { get; set; }

        [JsonProperty("measurement_description")]
        public string MeasurementDescription { get; set; }

        [JsonProperty("metric_serving_amount")]
        public decimal MetricServingAmount { get; set; }

        [JsonProperty("metric_serving_unit")]
        public string MetricServingUnit { get; set; }

        [JsonProperty("monounsaturated_fat")]
        public decimal MonounsaturatedFat { get; set; }

        [JsonProperty("number_of_units")]
        public decimal NumberOfUnits { get; set; }

        [JsonProperty("polyunsaturated_fat")]
        public decimal PolyunsaturatedFat { get; set; }

        [JsonProperty("potassium")]
        public decimal Potassium { get; set; }

        [JsonProperty("protein")]
        public decimal Protein { get; set; }

        [JsonProperty("saturated_fat")]
        public decimal SaturatedFat { get; set; }

        [JsonProperty("serving_description")]
        public string Description { get; set; }

        [JsonProperty("serving_id")]
        public long Id { get; set; }

        [JsonProperty("serving_url")]
        public string Url { get; set; }

        [JsonProperty("sodium")]
        public decimal Sodium { get; set; }

        [JsonProperty("sugar")]
        public decimal Sugar { get; set; }

        [JsonProperty("trans_fat")]
        public decimal TransFat { get; set; }

        [JsonProperty("vitamin_a")]
        public decimal VitaminA { get; set; }

        [JsonProperty("vitamin_c")]
        public decimal VitaminC { get; set; }
    }
}
