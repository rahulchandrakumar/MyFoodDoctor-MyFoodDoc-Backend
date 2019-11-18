using AutoMapper;
using MyFoodDoc.App.Application.Mappings;
using System.Collections.Generic;

namespace MyFoodDoc.App.Application.Models
{
    public class DiaryEntryDto : IMapFrom<(ICollection<DiaryEntryDtoMeal>, DiaryEntryDtoExercise, DiaryEntryDtoLiquid)>
    {
        public ICollection<DiaryEntryDtoMeal> Meals { get; set; }

        public DiaryEntryDtoExercise Exercise { get; set; }

        public DiaryEntryDtoLiquid Liquid { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<(ICollection<DiaryEntryDtoMeal>, DiaryEntryDtoExercise, DiaryEntryDtoLiquid), DiaryEntryDto>()
                .ForMember(d => d.Meals, opt => opt.MapFrom(s => s.Item1))
                .ForMember(d => d.Exercise, opt => opt.MapFrom(s => s.Item2))
                .ForMember(d => d.Liquid, opt => opt.MapFrom(s => s.Item3));
        }
    }
}