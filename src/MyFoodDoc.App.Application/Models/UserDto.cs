using AutoMapper;
using MyFoodDoc.App.Application.Mappings;
using MyFoodDoc.Application.Entities;
using MyFoodDoc.Application.Enums;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MyFoodDoc.App.Application.Models
{
    public class UserDto : IMapFrom<User>
    {
        public string Email { get; set; }

        public virtual bool IsAnamnesisCompleted => Gender != null && Height != null && (Indications != null || Motivations != null);

        [Obsolete]
        public virtual bool AnamnesisCompleted => IsAnamnesisCompleted;

        public int? Age { get; set; }

        public Gender? Gender { get; set; }

        public decimal? Height { get; set; }

        public int? InsuranceId { get; set; }

        public bool HasSubscription { get; set; }

        public DateTime? HasSubscriptionUpdated { get; set; }

        public ICollection<string> Motivations { get; set; }

        public ICollection<string> Indications { get; set; }

        public ICollection<string> Diets { get; set; }

        public void Mapping(Profile profile) 
        {
            profile.CreateMap<User, UserDto>()
                .ForMember(x => x.Age, opt => opt.MapFrom(src => src.Birthday == null ? null : (int?)(DateTime.UtcNow.Year - src.Birthday.Value.Year)))
                .ForMember(x => x.HasSubscription, opt => opt.MapFrom(src => src.HasValidSubscription != null && src.HasValidSubscription.Value))
                .ForMember(x => x.HasSubscriptionUpdated, opt => opt.MapFrom(src => src.SubscriptionUpdated))
                .ForMember(x => x.Indications, opt => opt.MapFrom(src => src.Indications.Select(x => x.Indication.Key)))
                .ForMember(x => x.Motivations, opt => opt.MapFrom(src => src.Motivations.Select(x => x.Motivation.Key)))
                .ForMember(x => x.Diets, opt => opt.MapFrom(src => src.Diets.Select(x => x.Diet.Key)));
        }
    }
}
