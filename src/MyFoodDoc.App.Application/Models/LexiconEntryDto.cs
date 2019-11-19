using AutoMapper;
using MyFoodDoc.Application.Entites;
using MyFoodDoc.App.Application.Mappings;

namespace MyFoodDoc.App.Application.Models
{
    public class LexiconEntryDto : IMapFrom<LexiconEntry>
    {
        public int Id { get; set; }

        public string Title { get; set; }

        public string ImageUrl { get; set; }

        public string Text { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<LexiconEntry, LexiconEntryDto>()
                .ForMember(d => d.Title, opt => opt.MapFrom(s => s.TitleLong))
                .ForMember(d => d.ImageUrl, opt => opt.MapFrom(s => s.Image.Url));
                
        }
    }
}