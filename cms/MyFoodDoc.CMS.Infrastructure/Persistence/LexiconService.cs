using MyFoodDoc.CMS.Application.Models;
using MyFoodDoc.CMS.Application.Persistence;
using MyFoodDoc.CMS.Infrastructure.Mock;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyFoodDoc.CMS.Infrastructure.Persistence
{
    public class LexiconService : ILexiconService
    {
        public async Task<LexiconModel> AddItem(LexiconModel item)
        {
            item.Id = LexiconMock.Default.Count == 0 ? 0 : (LexiconMock.Default.Max(u => u.Id) + 1);
            LexiconMock.Default.Add(item);
            return await Task.FromResult(item);
        }

        public async Task<bool> DeleteItem(int id)
        {
            var user = LexiconMock.Default.FirstOrDefault(u => u.Id == id);

            if (user == null)
                return await Task.FromResult(false);

            LexiconMock.Default.Remove(user);
            return await Task.FromResult(true);
        }

        public async Task<LexiconModel> GetItem(int id)
        {
            return await Task.FromResult(LexiconMock.Default.FirstOrDefault(u => u.Id == id));
        }

        public async Task<IList<LexiconModel>> GetItems()
        {
            return await Task.FromResult(LexiconMock.Default);
        }

        public async Task<LexiconModel> UpdateItem(LexiconModel item)
        {
            var itemModel = LexiconMock.Default.FirstOrDefault(u => u.Id == item.Id);

            if (itemModel == null)
                return null;

            LexiconMock.Default.Remove(itemModel);
            LexiconMock.Default.Add(item);
            return await Task.FromResult(item);
        }
    }
}
