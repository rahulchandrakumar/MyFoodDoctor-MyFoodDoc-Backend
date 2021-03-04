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
    public class ChoicesController : ControllerBase
    {
        private readonly IChoiceService _choiceService;
        private readonly IHubContext<EditStateHub> _hubContext;
        private const string _groupName = "choices";

        public ChoicesController(IChoiceService choiceService, IHubContext<EditStateHub> hubContext)
        {
            this._choiceService = choiceService;
            this._hubContext = hubContext;
        }

        // GET: api/v1/Choices
        [HttpGet]
        public async Task<object> Get([FromQuery] ChoicesGetPayload payload, CancellationToken cancellationToken = default)
        {
            var paginatedItems = await _choiceService.GetItems(payload.QuestionId, payload.Take, payload.Skip,
                payload.Search, cancellationToken);

            return new
            {
                values = paginatedItems.Items.Select(Choice.FromModel),
                total = paginatedItems.TotalCount
            };
        }

        // GET: api/v1/Choices/5
        [HttpGet("{id}")]
        public async Task<Choice> Get(int id, CancellationToken cancellationToken = default)
        {
            return Choice.FromModel(await _choiceService.GetItem(id, cancellationToken));
        }

        // POST: api/v1/Choices
        [HttpPost]
        public async Task Post([FromBody] Choice item, CancellationToken cancellationToken = default)
        {
            var model = await _choiceService.AddItem(item.ToModel(), cancellationToken);
            await _hubContext.Clients.Group(_groupName).SendAsync(EditStateHub.ItemAddedMsg, _groupName, model.Id, cancellationToken);
        }

        // PUT: api/v1/Choices
        [HttpPut()]
        public async Task Put([FromBody] Choice item, CancellationToken cancellationToken = default)
        {
            await _choiceService.UpdateItem(item.ToModel(), cancellationToken);
            await _hubContext.Clients.Group(_groupName).SendAsync(EditStateHub.ItemUpdatedMsg, _groupName, item.Id, cancellationToken);
        }

        // DELETE: api/v1/Choices/5
        [HttpDelete("{id}")]
        public async Task Delete(int id, CancellationToken cancellationToken = default)
        {
            await _choiceService.DeleteItem(id, cancellationToken);
            await _hubContext.Clients.Group(_groupName).SendAsync(EditStateHub.ItemDeletedMsg, _groupName, id, cancellationToken);
        }
    }
}
