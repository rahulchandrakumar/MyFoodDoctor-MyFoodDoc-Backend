using System.Collections.Generic;
using AutoMapper;
using MyFoodDoc.Application.Enums;
using MyFoodDoc.Application.Entities;
using MyFoodDoc.App.Application.Mappings;
using System;
using MyFoodDoc.Application.Entities.Diary;

namespace MyFoodDoc.App.Application.Models
{
    public class DiaryEntryDtoMeal : IMapFrom<Meal>
    {
        public int Id { get; set; }

        public TimeSpan Time { get; set; }

        public MealType Type { get; set; }

        public int? Mood { get; set; }

        public ICollection<DiaryEntryDtoMealIngredient> Ingredients { get; set; }

        public void Mapping(Profile profile) => profile.CreateMap(typeof(Meal), GetType());
    }
}
