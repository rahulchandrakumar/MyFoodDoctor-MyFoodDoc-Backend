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
    [Authorize(Roles = "Editor")]
    [Route("api/v1/[controller]")]
    [ApiController]
    public class LexiconController : ControllerBase
    {
        private readonly ILexiconService _lexiconService;
        private readonly IHubContext<EditStateHub> _hubContext;
        private const string _groupName = "lexicon";

        public LexiconController(ILexiconService lexiconService, IHubContext<EditStateHub> hubContext)
        {
            this._lexiconService = lexiconService;
            this._hubContext = hubContext;
        }

        // GET: api/v1/Users
        [HttpGet]
        public async Task<object> Get([FromQuery] LexiconGetPayload payload, CancellationToken cancellationToken = default)
        {
            return new
            {
                values = (await _lexiconService.GetItems(payload.CategoryId, payload.Take, payload.Skip, payload.Search, cancellationToken)).Select(LexiconItem.FromModel),
                total = await _lexiconService.GetItemsCount(payload.CategoryId, payload.Search, cancellationToken)
            };
        }

        // GET: api/v1/Users/5
        [HttpGet("{id}")]
        public async Task<LexiconItem> Get(int id, CancellationToken cancellationToken = default)
        {
            return LexiconItem.FromModel(await _lexiconService.GetItem(id, cancellationToken));
        }

        // POST: api/v1/Users
        [HttpPost]
        public async Task Post([FromBody] LexiconItem item, CancellationToken cancellationToken = default)
        {
            var model = await _lexiconService.AddItem(item.ToModel(), cancellationToken);
            await _hubContext.Clients.Group(_groupName).SendAsync(EditStateHub.ItemAddedMsg, _groupName, model.Id, cancellationToken);
        }

        // PUT: api/v1/Users
        [HttpPut()]
        public async Task Put([FromBody] LexiconItem item, CancellationToken cancellationToken = default)
        {
            await _lexiconService.UpdateItem(item.ToModel(), cancellationToken);
            await _hubContext.Clients.Group(_groupName).SendAsync(EditStateHub.ItemUpdatedMsg, _groupName, item.Id, cancellationToken);
        }

        // DELETE: api/v1/Users/5
        [HttpDelete("{id}")]
        public async Task Delete(int id, CancellationToken cancellationToken = default)
        {
            await _lexiconService.DeleteItem(id, cancellationToken);
            await _hubContext.Clients.Group(_groupName).SendAsync(EditStateHub.ItemDeletedMsg, _groupName, id, cancellationToken);
        }
    }
}
