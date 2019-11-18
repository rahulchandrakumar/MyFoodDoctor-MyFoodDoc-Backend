using AutoMapper;
using MyFoodDoc.Application.Entites;
using MyFoodDoc.Application.Enums;
using MyFoodDoc.Core.Mappings;
using System;
using System.Collections.Generic;
using Entity = MyFoodDoc.Application.Entites;

namespace MyFoodDoc.Api.Models
{
    public class UserDto : IMapFrom<Entity.User>
    {
        public string Email { get; set; }

        public virtual bool IsAnamnesisCompleted => Gender != null && Height != null && (Indications != null || Motivations != null);

        [Obsolete]
        public virtual bool AnamnesisCompleted => IsAnamnesisCompleted;

        public string Birthday { get; set; }

        public Gender? Gender { get; set; }

        public int? Height { get; set; }

        public int? InsuranceId { get; set; }

        public ICollection<string> Motivations { get; set; }

        public ICollection<string> Indications { get; set; }

        public ICollection<string> Diet { get; set; }

        public void Mapping(Profile profile) => profile.CreateMap(typeof(Entity.User), GetType());
    }
}
