using System;
using System.Collections.Generic;
using System.Text;
using AutoMapper;
using MyFoodDoc.App.Application.Mappings;
using MyFoodDoc.Application.Entities;

namespace MyFoodDoc.App.Application.Models
{
    public class DiaryEntryDtoIngredient : IMapFrom<Ingredient>
    {
        public int Id { get; set; }

        public long FoodId { get; set; }

        public string FoodName { get; set; }

        public long ServingId { get; set; }

        public string ServingDescription { get; set; }

        public decimal MetricServingAmount { get; set; }

        public string MetricServingUnit { get; set; }

        public string MeasurementDescription { get; set; }

        public void Mapping(Profile profile) => profile.CreateMap(typeof(Ingredient), GetType());
    }
}
