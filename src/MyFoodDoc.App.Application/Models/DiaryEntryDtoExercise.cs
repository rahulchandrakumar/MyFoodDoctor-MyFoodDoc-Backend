using AutoMapper;
using MyFoodDoc.Application.Entities;
using MyFoodDoc.App.Application.Mappings;
using System;
using MyFoodDoc.Application.Entities.Diary;

namespace MyFoodDoc.App.Application.Models
{
    public class DiaryEntryDtoExercise : IMapFrom<Exercise>
    {
        public int Duration { get; set; } = 0;

        public TimeSpan? LastAdded { get; set; }

        public void Mapping(Profile profile) => profile.CreateMap(typeof(Exercise), GetType());
    }
}