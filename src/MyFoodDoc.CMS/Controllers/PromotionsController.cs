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
    public class PromotionsController : ControllerBase
    {
        private readonly IPromotionService _promotionService;
        private readonly IHubContext<EditStateHub> _hubContext;
        private const string _groupName = "promotions";

        public PromotionsController(IPromotionService promotionService, IHubContext<EditStateHub> hubContext)
        {
            this._promotionService = promotionService;
            this._hubContext = hubContext;
        }

        // GET: api/v1/Users
        [HttpGet]
        public async Task<object> Get([FromQuery] WebPagesGetPayload payload, CancellationToken cancellationToken = default)
        {
            var paginatedItems = await _promotionService.GetItems(payload.Take, payload.Skip, payload.Search, cancellationToken);

            return new
            {
                values = paginatedItems.Items.Select(Promotion.FromModel),
                total = paginatedItems.TotalCount
            };
        }

        // GET: api/v1/Users/5
        [HttpGet("{id}")]
        public async Task<Promotion> Get(int id, CancellationToken cancellationToken = default)
        {
            return Promotion.FromModel(await _promotionService.GetItem(id, cancellationToken));
        }

        // POST: api/v1/Users
        [HttpPost]
        public async Task Post([FromBody] Promotion item, CancellationToken cancellationToken = default)
        {
            var model = await _promotionService.AddItem(item.ToModel(), cancellationToken);
            await _hubContext.Clients.Group(_groupName).SendAsync(EditStateHub.ItemAddedMsg, _groupName, model.Id, cancellationToken);
        }

        // PUT: api/v1/Users
        [HttpPut()]
        public async Task Put([FromBody] Promotion item, CancellationToken cancellationToken = default)
        {
            await _promotionService.UpdateItem(item.ToModel(), cancellationToken);
            await _hubContext.Clients.Group(_groupName).SendAsync(EditStateHub.ItemUpdatedMsg, _groupName, item.Id, cancellationToken);
        }

        // DELETE: api/v1/Users/5
        [HttpDelete("{id}")]
        public async Task Delete(int id, CancellationToken cancellationToken = default)
        {
            await _promotionService.DeleteItem(id, cancellationToken);
            await _hubContext.Clients.Group(_groupName).SendAsync(EditStateHub.ItemDeletedMsg, _groupName, id, cancellationToken);
        }
    }
}
