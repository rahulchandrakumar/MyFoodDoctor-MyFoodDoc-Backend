using MyFoodDoc.CMS.Application.Models;
using MyFoodDoc.CMS.Application.Persistence.Base;

namespace MyFoodDoc.CMS.Application.Persistence
{
    public interface ILexiconService : IServiceBasePaginatedRead<LexiconModel, int>, IServiceBaseWrite<LexiconModel, int>
    {
    }
}
