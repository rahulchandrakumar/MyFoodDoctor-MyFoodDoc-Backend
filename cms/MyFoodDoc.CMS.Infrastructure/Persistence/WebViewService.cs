using MyFoodDoc.CMS.Application.Models;
using MyFoodDoc.CMS.Application.Persistence;
using MyFoodDoc.CMS.Infrastructure.Mock;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace MyFoodDoc.CMS.Infrastructure.Persistence
{
    public class WebViewService : IWebViewService
    {
        public async Task<WebViewModel> AddItem(WebViewModel item, CancellationToken cancellationToken = default)
        {
            item.Id = WebViewMock.Default.Count == 0 ? 0 : (WebViewMock.Default.Max(u => u.Id) + 1);
            WebViewMock.Default.Add(item);
            return await Task.FromResult(item);
        }

        public async Task<bool> DeleteItem(int id, CancellationToken cancellationToken = default)
        {
            var user = WebViewMock.Default.FirstOrDefault(u => u.Id == id);

            if (user == null)
                return await Task.FromResult(false);

            WebViewMock.Default.Remove(user);
            return await Task.FromResult(true);
        }

        public async Task<WebViewModel> GetItem(int id, CancellationToken cancellationToken = default)
        {
            return await Task.FromResult(WebViewMock.Default.FirstOrDefault(u => u.Id == id));
        }

        public async Task<IList<WebViewModel>> GetItems(CancellationToken cancellationToken = default)
        {
            return await Task.FromResult(WebViewMock.Default);
        }

        public async Task<WebViewModel> UpdateItem(WebViewModel item, CancellationToken cancellationToken = default)
        {
            var user = WebViewMock.Default.FirstOrDefault(u => u.Id == item.Id);

            if (user == null)
                return null;

            WebViewMock.Default.Remove(user);
            WebViewMock.Default.Add(item);
            return await Task.FromResult(item);
        }
    }
}
