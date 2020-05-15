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
    public class TargetsController
    {
        private readonly ITargetService _targetService;
        private readonly IHubContext<EditStateHub> _hubContext;
        private const string _groupName = "targets";

        public TargetsController(ITargetService targetService, IHubContext<EditStateHub> hubContext)
        {
            this._targetService = targetService;
            this._hubContext = hubContext;
        }

        // GET: api/v1/Targets
        [HttpGet]
        public async Task<object> Get([FromQuery] TargetsGetPayload payload, CancellationToken cancellationToken = default)
        {
            return new
            {
                values = (await _targetService.GetItems(payload.OptimizationAreaId, payload.Take, payload.Skip, payload.Search, cancellationToken)).Select(Target.FromModel),
                total = await _targetService.GetItemsCount(payload.OptimizationAreaId, payload.Search, cancellationToken)
            };
        }

        // GET: api/v1/Targets/5
        [HttpGet("{id}")]
        public async Task<Target> Get(int id, CancellationToken cancellationToken = default)
        {
            return Target.FromModel(await _targetService.GetItem(id, cancellationToken));
        }

        // POST: api/v1/Targets
        [HttpPost]
        public async Task Post([FromBody] Target item, CancellationToken cancellationToken = default)
        {
            var model = await _targetService.AddItem(item.ToModel(), cancellationToken);
            await _hubContext.Clients.Group(_groupName).SendAsync(EditStateHub.ItemAddedMsg, _groupName, model.Id, cancellationToken);
        }

        // PUT: api/v1/Targets
        [HttpPut()]
        public async Task Put([FromBody] Target item, CancellationToken cancellationToken = default)
        {
            await _targetService.UpdateItem(item.ToModel(), cancellationToken);
            await _hubContext.Clients.Group(_groupName).SendAsync(EditStateHub.ItemUpdatedMsg, _groupName, item.Id, cancellationToken);
        }

        // DELETE: api/v1/Targets/5
        [HttpDelete("{id}")]
        public async Task Delete(int id, CancellationToken cancellationToken = default)
        {
            await _targetService.DeleteItem(id, cancellationToken);
            await _hubContext.Clients.Group(_groupName).SendAsync(EditStateHub.ItemDeletedMsg, _groupName, id, cancellationToken);
        }
    }
}
