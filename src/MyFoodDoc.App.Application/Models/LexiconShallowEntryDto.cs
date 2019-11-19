using AutoMapper;
using MyFoodDoc.Core.Mappings;
using Entity = MyFoodDoc.Application.Entites;

namespace MyFoodDoc.Api.Models
{
    public class LexiconShallowEntry : IMapFrom<Entity.LexiconEntry>
    {
        public int Id { get; set; }

        public string Title { get; set; }

        public virtual void Mapping(Profile profile)
        {
            profile.CreateMap<Entity.LexiconEntry, LexiconShallowEntry>()
                .ForMember(d => d.Title, opt => opt.MapFrom(s => s.TitleShort));
        }
    }
}
