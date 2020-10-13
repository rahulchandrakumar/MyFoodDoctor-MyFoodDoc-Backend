using System;
using System.Collections.Generic;
using System.Text;
using AutoMapper;
using MyFoodDoc.App.Application.Mappings;
using MyFoodDoc.Application.Entities;

namespace MyFoodDoc.App.Application.Models
{
    public class LexiconEntryDto : IMapFrom<LexiconEntry>
    {
        public int Id { get; set; }

        public string TitleShort { get; set; }

        public string TitleLong { get; set; }

        public string Text { get; set; }

        public string ImageUrl { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<LexiconEntry, LexiconEntryDto>()
                .ForMember(d => d.ImageUrl, opt => opt.MapFrom(s => s.Image.Url));
        }
    }
}
