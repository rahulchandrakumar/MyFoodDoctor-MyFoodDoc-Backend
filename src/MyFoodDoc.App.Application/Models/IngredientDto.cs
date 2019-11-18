using AutoMapper;
using MyFoodDoc.Application.Entites;
using MyFoodDoc.App.Application.Mappings;

namespace MyFoodDoc.App.Application.Models
{
    public class IngredientDto : IMapFrom<Ingredient>
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public void Mapping(Profile profile) => profile.CreateMap(typeof(Ingredient), GetType());
    }
}