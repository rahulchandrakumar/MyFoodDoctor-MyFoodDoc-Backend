using AutoMapper;
using MyFoodDoc.App.Application.Mappings;
using MyFoodDoc.Application.Entities;
using MyFoodDoc.Application.Entities.Diary;

namespace MyFoodDoc.App.Application.Models
{
    public class DiaryEntryDtoMealIngredient : IMapFrom<MealIngredient>
    {
        public DiaryEntryDtoIngredient Ingredient { get; set; }

        public decimal Amount { get; set; }

        public void Mapping(Profile profile) => profile.CreateMap(typeof(MealIngredient), GetType());
    }
}