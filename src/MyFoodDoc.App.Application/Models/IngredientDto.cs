using System;
using AutoMapper;
using MyFoodDoc.Application.Entites;
using MyFoodDoc.App.Application.Mappings;
using Newtonsoft.Json;

namespace MyFoodDoc.App.Application.Models
{
    public class IngredientDto : IMapFrom<Ingredient>
    {
        public long ServingId { get; set; }

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

        public void Mapping(Profile profile) => profile.CreateMap(typeof(Ingredient), GetType());
    }
}