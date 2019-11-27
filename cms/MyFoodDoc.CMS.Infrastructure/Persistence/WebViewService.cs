using Microsoft.EntityFrameworkCore;
using MyFoodDoc.Application.Abstractions;
using MyFoodDoc.CMS.Application.Models;
using MyFoodDoc.CMS.Application.Persistence;
using MyFoodDoc.CMS.Infrastructure.AzureBlob;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace MyFoodDoc.CMS.Infrastructure.Persistence
{
    public class WebViewService : IWebViewService
    {
        private readonly IApplicationContext _context;
        private readonly IWebViewBlobService _webViewBlobService;

        public WebViewService(IApplicationContext context, IWebViewBlobService webViewBlobService)
        {
            this._context = context;
            this._webViewBlobService = webViewBlobService;
        }

        public Task<WebViewModel> AddItem(WebViewModel item, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public Task<bool> DeleteItem(int id, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public async Task<WebViewModel> GetItem(int id, CancellationToken cancellationToken = default)
        {
            return WebViewModel.FromEntity(await _context.WebPages.FindAsync(id));
        }

        public async Task<IList<WebViewModel>> GetItems(CancellationToken cancellationToken = default)
        {
            return (await _context.WebPages.ToListAsync()).Select(WebViewModel.FromEntity).ToList();
        }

        public async Task<WebViewModel> UpdateItem(WebViewModel item, CancellationToken cancellationToken = default)
        {
            var webView = await _context.WebPages.FindAsync(item.Id);

            if (webView == null)
                return null;

            if (await _webViewBlobService.UpdateFile(item.Text, item.Url, cancellationToken))
            {
                _context.Entry(webView).CurrentValues.SetValues(item.ToEntity());

                await _context.SaveChangesAsync(cancellationToken);
            }

            return WebViewModel.FromEntity(webView);
        }
    }
}
