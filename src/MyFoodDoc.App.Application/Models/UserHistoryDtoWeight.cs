using AutoMapper;
using MyFoodDoc.App.Application.Mappings;
using MyFoodDoc.Application.Entities.TrackedValues;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MyFoodDoc.App.Application.Models
{
    public class UserHistoryDtoWeight : IMapFrom<IEnumerable<UserWeight>>
    {
        public HistoryEntry Initial { get; set; }

        public IEnumerable<HistoryEntry> History { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<UserWeight, HistoryEntry>();
            profile.CreateMap<IEnumerable<UserWeight>, UserHistoryDtoWeight>()
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