using AutoMapper;
using MyFoodDoc.Core.Mappings;
using Entity = MyFoodDoc.Application.Entites;

namespace MyFoodDoc.Api.Models
{
    public class Ingredient : IMapFrom<Entity.Ingredient>
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public void Mapping(Profile profile) => profile.CreateMap(typeof(Entity.Ingredient), GetType());
    }
}