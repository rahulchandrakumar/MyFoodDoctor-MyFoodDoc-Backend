using MyFoodDoc.CMS.Application.Models;
using MyFoodDoc.CMS.Application.Persistence.Base;

namespace MyFoodDoc.CMS.Application.Persistence
{
    public interface IChapterService : IServiceBasePaginatedChildrenRead<ChapterModel, int, int>, IServiceBaseWrite<ChapterModel, int>
    {
    }
}