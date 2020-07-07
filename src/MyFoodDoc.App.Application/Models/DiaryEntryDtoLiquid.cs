using AutoMapper;
using MyFoodDoc.Application.Entities;
using MyFoodDoc.App.Application.Mappings;
using System;

namespace MyFoodDoc.App.Application.Models
{
    public class DiaryEntryDtoLiquid : IMapFrom<Liquid>
    {
        public int Amount { get; set; } = 0;

        public int PredefinedAmount { get; set; } = 0;

        public TimeSpan? LastAdded { get; set; }

        public void Mapping(Profile profile) => profile.CreateMap(typeof(Liquid), GetType());
    }
}