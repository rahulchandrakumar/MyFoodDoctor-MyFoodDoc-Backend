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
    public class ScalesController : ControllerBase
    {
        private readonly IScaleService _scaleService;
        private readonly IHubContext<EditStateHub> _hubContext;
        private const string _groupName = "scales";

        public ScalesController(IScaleService scaleService, IHubContext<EditStateHub> hubContext)
        {
            this._scaleService = scaleService;
            this._hubContext = hubContext;
        }

        // GET: api/v1/Scales
        [HttpGet]
        public async Task<object> Get([FromQuery] ScalesGetPayload payload, CancellationToken cancellationToken = default)
        {
            var paginatedItems = await _scaleService.GetItems(payload.Take, payload.Skip, payload.Search, cancellationToken);

            return new
            {
                values = paginatedItems.Items.Select(Scale.FromModel),
                total = paginatedItems.TotalCount
            };
        }

        // GET: api/v1/Scales/5
        [HttpGet("{id}")]
        public async Task<Scale> Get(int id, CancellationToken cancellationToken = default)
        {
            return Scale.FromModel(await _scaleService.GetItem(id, cancellationToken));
        }

        // POST: api/v1/Scales
        [HttpPost]
        public async Task Post([FromBody] Scale item, CancellationToken cancellationToken = default)
        {
            var model = await _scaleService.AddItem(item.ToModel(), cancellationToken);
            await _hubContext.Clients.Group(_groupName).SendAsync(EditStateHub.ItemAddedMsg, _groupName, model.Id, cancellationToken);
        }

        // PUT: api/v1/Scales
        [HttpPut()]
        public async Task Put([FromBody] Scale item, CancellationToken cancellationToken = default)
        {
            await _scaleService.UpdateItem(item.ToModel(), cancellationToken);
            await _hubContext.Clients.Group(_groupName).SendAsync(EditStateHub.ItemUpdatedMsg, _groupName, item.Id, cancellationToken);
        }

        // DELETE: api/v1/Scales/5
        [HttpDelete("{id}")]
        public async Task Delete(int id, CancellationToken cancellationToken = default)
        {
            await _scaleService.DeleteItem(id, cancellationToken);
            await _hubContext.Clients.Group(_groupName).SendAsync(EditStateHub.ItemDeletedMsg, _groupName, id, cancellationToken);
        }
    }
}
