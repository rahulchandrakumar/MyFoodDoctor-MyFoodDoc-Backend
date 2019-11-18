using AutoMapper;
using MyFoodDoc.Core.Mappings;
using Entity = MyFoodDoc.Application.Entites;

namespace MyFoodDoc.Api.Models
{
    public class DiaryLiquid : IMapFrom<Entity.Liquid>
    {
        public int Amount { get; set; }

        public string LastAdded { get; set; }

        public void Mapping(Profile profile) => profile.CreateMap(typeof(Entity.Liquid), GetType());
    }
}