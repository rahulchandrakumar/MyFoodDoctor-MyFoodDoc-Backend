using System.Collections.Generic;
using AutoMapper;
using MyFoodDoc.Core.Mappings;
using Entity = MyFoodDoc.Application.Entites;

namespace MyFoodDoc.Api.Models
{
    public class DiaryEntry : IMapFrom<Entity.DiaryEntry>
    {
        public ICollection<DiaryMeal> Meals { get; set; }

        public DiaryExercise Exercise { get; set; }

        public DiaryLiquid Liquid { get; set; }

        public void Mapping(Profile profile) => profile.CreateMap(typeof(Entity.DiaryEntry), GetType());
    }
}