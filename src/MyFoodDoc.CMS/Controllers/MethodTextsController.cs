using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using MyFoodDoc.CMS.Application.Persistence;
using MyFoodDoc.CMS.Hubs;
using MyFoodDoc.CMS.Models.VM;
using MyFoodDoc.CMS.Payloads;

namespace MyFoodDoc.CMS.Controllers
{
    [Authorize(Roles = "Admin")]
    [Route("api/v1/[controller]")]
    [ApiController]
    public class MethodTextsController : ControllerBase
    {
        private readonly IMethodTextService _methodTextService;
        private readonly IHubContext<EditStateHub> _hubContext;
        private const string _groupName = "methodtexts";

        public MethodTextsController(IMethodTextService methodTextService, IHubContext<EditStateHub> hubContext)
        {
            this._methodTextService = methodTextService;
            this._hubContext = hubContext;
        }

        // GET: api/v1/MethodTexts
        [HttpGet]
        public async Task<object> Get([FromQuery] MethodTextsGetPayload payload, CancellationToken cancellationToken = default)
        {
            var paginatedItems = await _methodTextService.GetItems(payload.MethodId, payload.Take, payload.Skip, payload.Search, cancellationToken);

            return new
            {
                values = paginatedItems.Items.Select(MethodText.FromModel),
                total = paginatedItems.TotalCount
            };
        }

        // GET: api/v1/MethodTexts/5
        [HttpGet("{id}")]
        public async Task<MethodText> Get(int id, CancellationToken cancellationToken = default)
        {
            return MethodText.FromModel(await _methodTextService.GetItem(id, cancellationToken));
        }

        // POST: api/v1/MethodTexts
        [HttpPost]
        public async Task Post([FromBody] MethodText item, CancellationToken cancellationToken = default)
        {
            var model = await _methodTextService.AddItem(item.ToModel(), cancellationToken);
            await _hubContext.Clients.Group(_groupName).SendAsync(EditStateHub.ItemAddedMsg, _groupName, model.Id, cancellationToken);
        }

        // PUT: api/v1/MethodTexts
        [HttpPut()]
        public async Task Put([FromBody] MethodText item, CancellationToken cancellationToken = default)
        {
            await _methodTextService.UpdateItem(item.ToModel(), cancellationToken);
            await _hubContext.Clients.Group(_groupName).SendAsync(EditStateHub.ItemUpdatedMsg, _groupName, item.Id, cancellationToken);
        }

        // DELETE: api/v1/MethodTexts/5
        [HttpDelete("{id}")]
        public async Task Delete(int id, CancellationToken cancellationToken = default)
        {
            await _methodTextService.DeleteItem(id, cancellationToken);
            await _hubContext.Clients.Group(_groupName).SendAsync(EditStateHub.ItemDeletedMsg, _groupName, id, cancellationToken);
        }
    }
}
