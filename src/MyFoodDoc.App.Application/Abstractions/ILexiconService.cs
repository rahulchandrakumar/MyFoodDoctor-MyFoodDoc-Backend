using MyFoodDoc.Api.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MyFoodDoc.App.Application.Abstractions
{
    public interface ILexiconService
    {
        Task<IEnumerable<LexiconShallowEntry>> GetAll();

        Task<LexiconEntry> Get(int entryId);
    }
}
