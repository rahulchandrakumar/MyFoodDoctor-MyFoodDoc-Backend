using AutoMapper;
using MyFoodDoc.App.Application.Mappings;
using MyFoodDoc.Application.Entities.Diary;
using MyFoodDoc.Application.Enums;
using System;
using System.Collections.Generic;

namespace MyFoodDoc.App.Application.Models
{
    public class DiaryEntryDtoMeal : IMapFrom<Meal>
    {
        public int Id { get; set; }

        public TimeSpan Time { get; set; }

        public MealType Type { get; set; }

        public int? Mood { get; set; }

        public ICollection<DiaryEntryDtoMealIngredient> Ingredients { get; set; }

        public ICollection<MealFavouriteDto> Favourites { get; set; }

        public void Mapping(Profile profile) => profile.CreateMap(typeof(Meal), GetType());
    }
}
