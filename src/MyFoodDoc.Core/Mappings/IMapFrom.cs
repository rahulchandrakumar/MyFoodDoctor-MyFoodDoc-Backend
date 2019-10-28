using AutoMapper;

namespace MyFoodDoc.Core.Mappings
{
    public interface IMapFrom<T>
    {
        void Mapping(Profile profile);
    }
}
