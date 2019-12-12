using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using MyFoodDoc.CMS.Application.Persistence;
using MyFoodDoc.CMS.Hubs;
using MyFoodDoc.CMS.Models.VM;
using MyFoodDoc.CMS.Payloads;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace MyFoodDoc.CMS.Controllers
{
    [Authorize(Roles = "Admin")]
    [Route("api/v1/[controller]")]
    [ApiController]
    public class WebPagesController : ControllerBase
    {
        private readonly IWebViewService _webViewService;
        private readonly IHubContext<EditStateHub> _hubContext;
        private const string _groupName = "webpages";

        public WebPagesController(IWebViewService webViewService, IHubContext<EditStateHub> hubContext)
        {
            this._webViewService = webViewService;
            this._hubContext = hubContext;
        }

        // GET: api/v1/Users
        [HttpGet]
        public async Task<object> Get([FromQuery] UsersGetPayload payload, CancellationToken cancellationToken = default)
        {
            return new
            {
                values = (await _webViewService.GetItems(payload.Take, payload.Skip, payload.Search, cancellationToken)).Select(WebViewItem.FromModel),
                total = await _webViewService.GetItemsCount(payload.Search, cancellationToken)
            };
        }

        // GET: api/v1/Users/5
        [HttpGet("{id}")]
        public async Task<WebViewItem> Get(int id, CancellationToken cancellationToken = default)
        {
            return WebViewItem.FromModel(await _webViewService.GetItem(id, cancellationToken));
        }

        // POST: api/v1/Users
        [HttpPost]
        public async Task Post([FromBody] WebViewItem item, CancellationToken cancellationToken = default)
        {
            var model = await _webViewService.AddItem(item.ToModel(), cancellationToken);
            await _hubContext.Clients.Group(_groupName).SendAsync(EditStateHub.ItemAddedMsg, _groupName, model.Id, cancellationToken);
        }

        // PUT: api/v1/Users
        [HttpPut()]
        public async Task Put([FromBody] WebViewItem item, CancellationToken cancellationToken = default)
        {
            await _webViewService.UpdateItem(item.ToModel(), cancellationToken);
            await _hubContext.Clients.Group(_groupName).SendAsync(EditStateHub.ItemUpdatedMsg, _groupName, item.Id, cancellationToken);
        }

        // DELETE: api/v1/Users/5
        [HttpDelete("{id}")]
        public async Task Delete(int id, CancellationToken cancellationToken = default)
        {
            await _webViewService.DeleteItem(id, cancellationToken);
            await _hubContext.Clients.Group(_groupName).SendAsync(EditStateHub.ItemDeletedMsg, _groupName, id, cancellationToken);
        }
    }
}
