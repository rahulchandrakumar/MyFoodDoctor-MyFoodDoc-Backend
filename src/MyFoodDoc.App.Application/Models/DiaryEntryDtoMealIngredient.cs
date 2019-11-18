using AutoMapper;
using MyFoodDoc.App.Application.Mappings;
using MyFoodDoc.Application.Entites;

namespace MyFoodDoc.App.Application.Models
{
    public class DiaryEntryDtoMealIngredient : IMapFrom<MealIngredient>
    {
        public IngredientDto Ingredient { get; set; }

        public int Amount { get; set; }

        public void Mapping(Profile profile) => profile.CreateMap(typeof(MealIngredient), GetType());
    }
}