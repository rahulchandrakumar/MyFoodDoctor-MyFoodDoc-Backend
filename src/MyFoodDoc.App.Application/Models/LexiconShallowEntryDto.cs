using AutoMapper;
using MyFoodDoc.Application.Entities;
using MyFoodDoc.App.Application.Mappings;

namespace MyFoodDoc.App.Application.Models
{
    public class LexiconShallowEntryDto : IMapFrom<LexiconEntry>
    {
        public int Id { get; set; }

        public string TitleShort { get; set; }

        public string TitleLong { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<LexiconEntry, LexiconShallowEntryDto>();
        }
    }
}
