using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AutoMapper;
using MyFoodDoc.App.Application.Mappings;
using MyFoodDoc.Application.Entities;

namespace MyFoodDoc.App.Application.Models
{
    public class LexiconCategoryDto : IMapFrom<LexiconCategory>
    {
        public int Id { get; set; }

        public string Title { get; set; }

        public string ImageUrl { get; set; }

        public ICollection<LexiconShallowEntryDto> Entries { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<LexiconCategory, LexiconCategoryDto>()
                .ForMember(d => d.ImageUrl, opt => opt.MapFrom(s => s.Image.Url));
        }
    }
}
