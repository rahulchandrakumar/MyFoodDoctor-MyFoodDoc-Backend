using AutoMapper;
using MyFoodDoc.Core.Mappings;
using Entity = MyFoodDoc.Application.Entites;

namespace MyFoodDoc.Api.Models
{
    public class DiaryMealIngredient : IMapFrom<Entity.DiaryMealIngredient>
    {
        public Ingredient Ingredient { get; set; }

        public int Amount { get; set; }

        public void Mapping(Profile profile) => profile.CreateMap(typeof(Entity.DiaryMealIngredient), GetType());
    }
}