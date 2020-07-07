using AutoMapper;
using MyFoodDoc.App.Application.Mappings;
using MyFoodDoc.Application.Entities;
using System.Linq;

namespace MyFoodDoc.App.Application.Models
{
    public class UserHistoryDto : IMapFrom<User>
    {
        public UserHistoryDtoWeight Weight { get; set; }

        public UserHistoryDtoAbdominalGirth AbdominalGirth { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<User, UserHistoryDto>()
                .ForMember(d => d.Weight, opt => opt.MapFrom(s => s.WeightHistory))
                .ForMember(d => d.AbdominalGirth, opt => opt.MapFrom(s => s.AbdominalGirthHistory));
        }
    }
}