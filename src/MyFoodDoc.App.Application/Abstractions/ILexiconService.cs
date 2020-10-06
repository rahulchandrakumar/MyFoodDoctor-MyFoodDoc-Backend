using MyFoodDoc.App.Application.Models;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace MyFoodDoc.App.Application.Abstractions
{
    public interface ILexiconService
    {
        Task<ICollection<LexiconCategoryDto>> GetAllAsync(CancellationToken cancellationToken = default);

        Task<LexiconEntryDto> GetAsync(int entryId, CancellationToken cancellationToken = default);
    }
}
