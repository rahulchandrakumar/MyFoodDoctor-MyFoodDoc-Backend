using AutoMapper;

namespace MyFoodDoc.App.Application.Mappings
{
    public interface IMapFrom<T>
    {
        void Mapping(Profile profile);
    }
}
