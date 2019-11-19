using System.Collections.Generic;
using AutoMapper;
using MyFoodDoc.Application.Enums;
using MyFoodDoc.Core.Mappings;
using Entity = MyFoodDoc.Application.Entites;

namespace MyFoodDoc.Api.Models
{
    public class DiaryMeal : IMapFrom<Entity.Meal>
    {
        public int Id { get; set; }

        public string Time { get; set; }

        public MealType Type { get; set; }

        public ICollection<DiaryMealIngredient> Ingredients { get; set; }

        public int Mood { get; set; }

        public void Mapping(Profile profile) => profile.CreateMap(typeof(Entity.Meal), GetType());
    }
}
