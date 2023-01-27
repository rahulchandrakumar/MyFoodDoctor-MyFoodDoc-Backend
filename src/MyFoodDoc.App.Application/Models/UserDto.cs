using AutoMapper;
using MyFoodDoc.App.Application.Mappings;
using MyFoodDoc.Application.Entities;
using MyFoodDoc.Application.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using MyFoodDoc.Application.Entities.Diary;
using MyFoodDoc.Application.Entities.TrackedValues;

namespace MyFoodDoc.App.Application.Models
{
    public class UserDto : IMapFrom<User>
    {
        public DateTime Created { get; set; }

        public string Email { get; set; }

        public virtual bool IsAnamnesisCompleted =>
            Gender != null && Height != null && (Indications != null || Motivations != null);

        public int? Age { get; set; }

        public Gender? Gender { get; set; }

        public decimal? Height { get; set; }

        public int? InsuranceId { get; set; }

        public bool HasSubscription { get; set; }

        public bool HasZPPSubscription { get; set; }

        public ICollection<string> Motivations { get; set; }

        public ICollection<string> Indications { get; set; }

        public ICollection<string> Diets { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<User, UserDto>()
                .ForMember(x => x.Created, opt => opt.MapFrom(src => src.Created.ToLocalTime().Date))
                .ForMember(x => x.Age,
                    opt => opt.MapFrom(src =>
                        src.Birthday == null ? null : (int?) (DateTime.UtcNow.Year - src.Birthday.Value.Year)))
                .ForMember(x => x.Indications, opt => opt.MapFrom(src => src.Indications.Select(x => x.Indication.Key)))
                .ForMember(x => x.Motivations, opt => opt.MapFrom(src => src.Motivations.Select(x => x.Motivation.Key)))
                .ForMember(x => x.Diets, opt => opt.MapFrom(src => src.Diets.Select(x => x.Diet.Key)));
        }
    }

    public class StatisticsUserDto : IMapFrom<User>
    {
        public DateTime Created { get; set; }

        public string Email { get; set; }

        public virtual bool IsAnamnesisCompleted =>
            Gender != null && Height != null && (Indications != null || Motivations != null);

        public int? Age { get; set; }

        public Gender? Gender { get; set; }

        public decimal? Height { get; set; }

        public int? InsuranceId { get; set; }

        public bool HasSubscription { get; set; }

        public bool HasZPPSubscription { get; set; }

        public ICollection<string> Motivations { get; set; }

        public ICollection<string> Indications { get; set; }

        public ICollection<string> Diets { get; set; }

        public UserWeight Weight { get; set; }
        public bool EatingDisorder { get; set; }
        public ICollection<MealDto> Meals { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<User, StatisticsUserDto>()
                .ForMember(x => x.Created, opt => opt.MapFrom(src => src.Created.ToLocalTime().Date))
                .ForMember(x => x.Age,
                    opt => opt.MapFrom(src =>
                        src.Birthday == null ? null : (int?) (DateTime.UtcNow.Year - src.Birthday.Value.Year)))
                .ForMember(x => x.Indications, opt => opt.MapFrom(src => src.Indications.Select(x => x.Indication.Key)))
                .ForMember(x => x.Motivations, opt => opt.MapFrom(src => src.Motivations.Select(x => x.Motivation.Key)))
                .ForMember(x => x.Diets, opt => opt.MapFrom(src => src.Diets.Select(x => x.Diet.Key)))
                .ForMember(x => x.Weight,
                    opt => opt.MapFrom(src => src.WeightHistory.OrderBy(w => w.Date).LastOrDefault(x => x.UserId == src.Id && x.Date <= DateTime.Now)))
                .ForMember(x => x.EatingDisorder,
                 opt => opt.MapFrom(src => src.Indications.Any(x => x.IndicationId == 5)))
                .ForMember(x => x.Meals, opt => 
                    opt.MapFrom(src => src.Meals.Select(m => new MealDto(m.Date, m.Type, m.Mood))) );
        }
    }

    public record MealDto(DateTime Date, MealType Type, int? Mood);
}