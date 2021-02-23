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
    public class MethodsController : ControllerBase
    {
        private readonly IMethodService _methodService;
        private readonly IHubContext<EditStateHub> _hubContext;
        private const string _groupName = "methods";

        public MethodsController(IMethodService methodService, IHubContext<EditStateHub> hubContext)
        {
            this._methodService = methodService;
            this._hubContext = hubContext;
        }

        // GET: api/v1/Methods
        [HttpGet]
        public async Task<object> Get([FromQuery] MethodsGetPayload payload, CancellationToken cancellationToken = default)
        {
            var paginatedItems = await _methodService.GetItems(payload.ParentId, payload.Take, payload.Skip, payload.Search,
                cancellationToken);

            return new
            {
                values = paginatedItems.Items.Select(Method.FromModel),
                total = paginatedItems.TotalCount
            };
        }

        // GET: api/v1/Methods/5
        [HttpGet("{id}")]
        public async Task<Method> Get(int id, CancellationToken cancellationToken = default)
        {
            return Method.FromModel(await _methodService.GetItem(id, cancellationToken));
        }

        // POST: api/v1/Methods
        [HttpPost]
        public async Task Post([FromBody] Method item, CancellationToken cancellationToken = default)
        {
            var model = await _methodService.AddItem(item.ToModel(), cancellationToken);
            await _hubContext.Clients.Group(_groupName).SendAsync(EditStateHub.ItemAddedMsg, _groupName, model.Id, cancellationToken);
        }

        // PUT: api/v1/Methods
        [HttpPut()]
        public async Task Put([FromBody] Method item, CancellationToken cancellationToken = default)
        {
            await _methodService.UpdateItem(item.ToModel(), cancellationToken);
            await _hubContext.Clients.Group(_groupName).SendAsync(EditStateHub.ItemUpdatedMsg, _groupName, item.Id, cancellationToken);
        }

        // DELETE: api/v1/Methods/5
        [HttpDelete("{id}")]
        public async Task Delete(int id, CancellationToken cancellationToken = default)
        {
            await _methodService.DeleteItem(id, cancellationToken);
            await _hubContext.Clients.Group(_groupName).SendAsync(EditStateHub.ItemDeletedMsg, _groupName, id, cancellationToken);
        }
    }
}
