using AutoMapper;
using MyFoodDoc.Application.Entites;
using MyFoodDoc.App.Application.Mappings;
using System;
using MyFoodDoc.Application.Entites.TrackedValus;

namespace MyFoodDoc.App.Application.Models
{
    public class UserHistoryDtoAbdominalGirth : IMapFrom<UserAbdominalGirth>
    {
        public DateTime Date { get; set; }

        public int Value { get; set; }

        public void Mapping(Profile profile) => profile.CreateMap(typeof(UserAbdominalGirth), GetType());
    }
}