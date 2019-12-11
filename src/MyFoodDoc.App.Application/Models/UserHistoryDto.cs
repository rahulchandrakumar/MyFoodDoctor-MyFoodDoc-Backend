using AutoMapper;
using MyFoodDoc.App.Application.Mappings;
using MyFoodDoc.Application.Entites;
using System.Collections.Generic;

namespace MyFoodDoc.App.Application.Models
{
    public class UserHistoryDto : IMapFrom<User>
    {
        public ICollection<UserHistoryDtoWeight> WeightHistory { get; set; }

        public ICollection<UserHistoryDtoAbdominalGirth> AbdominalGirthHistory { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<User, UserHistoryDto>()
                .ForMember(d => d.WeightHistory, opt => opt.MapFrom(s => s.WeightHistory))
                .ForMember(d => d.AbdominalGirthHistory, opt => opt.MapFrom(s => s.AbdominalGirthHistory));
        }
    }
}