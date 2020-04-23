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
    public class OptimizationAreasController
    {
        private readonly IOptimizationAreaService _optimizationAreaService;
        private readonly IHubContext<EditStateHub> _hubContext;
        private const string _groupName = "optimizationareas";

        public OptimizationAreasController(IOptimizationAreaService optimizationAreaService, IHubContext<EditStateHub> hubContext)
        {
            this._optimizationAreaService = optimizationAreaService;
            this._hubContext = hubContext;
        }

        // GET: api/v1/OptimizationAreas
        [HttpGet]
        public async Task<object> Get([FromQuery] OptimizationAreaGetPayload payload, CancellationToken cancellationToken = default)
        {
            return new
            {
                values = (await _optimizationAreaService.GetItems(payload.Take, payload.Skip, payload.Search, cancellationToken)).Select(OptimizationArea.FromModel),
                total = await _optimizationAreaService.GetItemsCount(payload.Search, cancellationToken)
            };
        }

        // GET: api/v1/OptimizationAreas/5
        [HttpGet("{id}")]
        public async Task<OptimizationArea> Get(int id, CancellationToken cancellationToken = default)
        {
            return OptimizationArea.FromModel(await _optimizationAreaService.GetItem(id, cancellationToken));
        }

        // POST: api/v1/OptimizationAreas
        [HttpPost]
        public async Task Post([FromBody] OptimizationArea item, CancellationToken cancellationToken = default)
        {
            var model = await _optimizationAreaService.AddItem(item.ToModel(), cancellationToken);
            await _hubContext.Clients.Group(_groupName).SendAsync(EditStateHub.ItemAddedMsg, _groupName, model.Id, cancellationToken);
        }

        // PUT: api/v1/OptimizationAreas
        [HttpPut()]
        public async Task Put([FromBody] OptimizationArea item, CancellationToken cancellationToken = default)
        {
            await _optimizationAreaService.UpdateItem(item.ToModel(), cancellationToken);
            await _hubContext.Clients.Group(_groupName).SendAsync(EditStateHub.ItemUpdatedMsg, _groupName, item.Id, cancellationToken);
        }

        // DELETE: api/v1/OptimizationAreas/5
        [HttpDelete("{id}")]
        public async Task Delete(int id, CancellationToken cancellationToken = default)
        {
            await _optimizationAreaService.DeleteItem(id, cancellationToken);
            await _hubContext.Clients.Group(_groupName).SendAsync(EditStateHub.ItemDeletedMsg, _groupName, id, cancellationToken);
        }
    }
}
