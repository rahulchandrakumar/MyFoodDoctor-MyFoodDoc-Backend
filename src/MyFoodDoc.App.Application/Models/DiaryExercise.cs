using AutoMapper;
using MyFoodDoc.Core.Mappings;
using Entity = MyFoodDoc.Application.Entites;

namespace MyFoodDoc.Api.Models
{
    public class DiaryExercise : IMapFrom<Entity.Exercise>
    {
        public int Duration { get; set; }

        public string LastAdded { get; set; }

        public void Mapping(Profile profile) => profile.CreateMap(typeof(Entity.Exercise), GetType());
    }
}