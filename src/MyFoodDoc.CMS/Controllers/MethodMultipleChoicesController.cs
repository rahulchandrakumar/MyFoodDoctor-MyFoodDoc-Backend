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
    [Authorize(Roles = "Editor")]
    [Route("api/v1/[controller]")]
    [ApiController]
    public class MethodMultipleChoicesController
    {
        private readonly IMethodMultipleChoiceService _methodMultipleChoiceService;
        private readonly IHubContext<EditStateHub> _hubContext;
        private const string _groupName = "methodmultiplechoices";

        public MethodMultipleChoicesController(IMethodMultipleChoiceService methodMultipleChoiceService, IHubContext<EditStateHub> hubContext)
        {
            this._methodMultipleChoiceService = methodMultipleChoiceService;
            this._hubContext = hubContext;
        }

        // GET: api/v1/MethodMultipleChoices
        [HttpGet]
        public async Task<object> Get([FromQuery] MethodMultipleChoicesGetPayload payload, CancellationToken cancellationToken = default)
        {
            return new
            {
                values = (await _methodMultipleChoiceService.GetItems(payload.MethodId, payload.Take, payload.Skip, payload.Search, cancellationToken)).Select(MethodMultipleChoice.FromModel),
                total = await _methodMultipleChoiceService.GetItemsCount(payload.MethodId, payload.Search, cancellationToken)
            };
        }

        // GET: api/v1/MethodMultipleChoices/5
        [HttpGet("{id}")]
        public async Task<MethodMultipleChoice> Get(int id, CancellationToken cancellationToken = default)
        {
            return MethodMultipleChoice.FromModel(await _methodMultipleChoiceService.GetItem(id, cancellationToken));
        }

        // POST: api/v1/MethodMultipleChoices
        [HttpPost]
        public async Task Post([FromBody] MethodMultipleChoice item, CancellationToken cancellationToken = default)
        {
            var model = await _methodMultipleChoiceService.AddItem(item.ToModel(), cancellationToken);
            await _hubContext.Clients.Group(_groupName).SendAsync(EditStateHub.ItemAddedMsg, _groupName, model.Id, cancellationToken);
        }

        // PUT: api/v1/MethodMultipleChoices
        [HttpPut()]
        public async Task Put([FromBody] MethodMultipleChoice item, CancellationToken cancellationToken = default)
        {
            await _methodMultipleChoiceService.UpdateItem(item.ToModel(), cancellationToken);
            await _hubContext.Clients.Group(_groupName).SendAsync(EditStateHub.ItemUpdatedMsg, _groupName, item.Id, cancellationToken);
        }

        // DELETE: api/v1/MethodMultipleChoices/5
        [HttpDelete("{id}")]
        public async Task Delete(int id, CancellationToken cancellationToken = default)
        {
            await _methodMultipleChoiceService.DeleteItem(id, cancellationToken);
            await _hubContext.Clients.Group(_groupName).SendAsync(EditStateHub.ItemDeletedMsg, _groupName, id, cancellationToken);
        }
    }
}
