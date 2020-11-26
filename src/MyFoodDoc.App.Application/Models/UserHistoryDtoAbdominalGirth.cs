using AutoMapper;
using MyFoodDoc.Application.Entities;
using MyFoodDoc.App.Application.Mappings;
using System;
using MyFoodDoc.Application.Entities.TrackedValues;
using System.Collections.Generic;
using System.Linq;
using MyFoodDoc.Core;

namespace MyFoodDoc.App.Application.Models
{
    public class UserHistoryDtoAbdominalGirth : IMapFrom<IEnumerable<UserAbdominalGirth>>
    {
        public HistoryEntry Initial { get; set; }

        public IEnumerable<HistoryEntry> History { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<UserAbdominalGirth, HistoryEntry>();
            profile.CreateMap<IEnumerable<UserAbdominalGirth>, UserHistoryDtoAbdominalGirth>()
                .ForMember(d => d.Initial, opt => opt.MapFrom(s => s.OrderBy(x => x.Date).FirstOrDefault()))
                .ForMember(d => d.History, opt => opt.MapFrom(s => s.OrderBy(x => x.Date)));
        }

        public class HistoryEntry
        {
            public DateTime Date { get; set; }

            public decimal Value { get; set; }
        }
    }
}