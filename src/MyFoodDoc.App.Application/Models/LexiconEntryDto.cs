using AutoMapper;
using MyFoodDoc.Core.Mappings;
using Entity = MyFoodDoc.Application.Entites;

namespace MyFoodDoc.Api.Models
{
    public class LexiconEntry : LexiconShallowEntry, IMapFrom<Entity.LexiconEntry>
    {
        public string ImageUrl { get; set; }

        public string Text { get; set; }

        public override void Mapping(Profile profile)
        {
            profile.CreateMap<Entity.LexiconEntry, LexiconShallowEntry>()
                .ForMember(d => d.Title, opt => opt.MapFrom(s => s.TitleLong));
        }
    }
}