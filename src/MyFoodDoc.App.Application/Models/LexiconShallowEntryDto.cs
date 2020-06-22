using AutoMapper;
using MyFoodDoc.Application.Entities;
using MyFoodDoc.App.Application.Mappings;

namespace MyFoodDoc.App.Application.Models
{
    public class LexiconShallowEntryDto : IMapFrom<LexiconEntry>
    {
        public int Id { get; set; }

        public string Title { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<LexiconEntry, LexiconShallowEntryDto>()
                .ForMember(d => d.Title, opt => opt.MapFrom(s => s.TitleShort));
        }
    }
}
