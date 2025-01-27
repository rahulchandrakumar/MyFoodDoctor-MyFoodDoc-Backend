﻿using Microsoft.AspNetCore.Authorization;
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
    [Authorize(Roles = "Viewer")]
    [Route("api/v1/[controller]")]
    [ApiController]
    public class PortionsController : ControllerBase
    {
        private readonly IIngredientService _ingredientService;
        private readonly IHubContext<EditStateHub> _hubContext;
        private const string _groupName = "portions";

        public PortionsController(IIngredientService ingredientService, IHubContext<EditStateHub> hubContext)
        {
            this._ingredientService = ingredientService;
            this._hubContext = hubContext;
        }

        // GET: api/v1/Portions
        [HttpGet]
        public async Task<object> Get([FromQuery] PortionsGetPayload payload, CancellationToken cancellationToken = default)
        {
            var paginatedItems = await _ingredientService.GetItems(payload.Take, payload.Skip, payload.Search,
                payload.Filter.ToModel(), cancellationToken);

            return new
            {
                values = paginatedItems.Items.Select(Ingredient.FromModel),
                total = paginatedItems.TotalCount
            };
        }

        // GET: api/v1/Portions/5
        [HttpGet("{id}")]
        public async Task<Ingredient> Get(int id, CancellationToken cancellationToken = default)
        {
            return Ingredient.FromModel(await _ingredientService.GetItem(id, cancellationToken));
        }

        // POST: api/v1/Portions
        [HttpPost]
        public async Task Post([FromBody] Ingredient item, CancellationToken cancellationToken = default)
        {
            var model = await _ingredientService.AddItem(item.ToModel(), cancellationToken);
            await _hubContext.Clients.Group(_groupName).SendAsync(EditStateHub.ItemAddedMsg, _groupName, model.Id, cancellationToken);
        }

        // PUT: api/v1/Portions
        [HttpPut()]
        public async Task Put([FromBody] Ingredient item, CancellationToken cancellationToken = default)
        {
            await _ingredientService.UpdateItem(item.ToModel(), cancellationToken);
            await _hubContext.Clients.Group(_groupName).SendAsync(EditStateHub.ItemUpdatedMsg, _groupName, item.Id, cancellationToken);
        }

        // DELETE: api/v1/Portions/5
        [HttpDelete("{id}")]
        public async Task Delete(int id, CancellationToken cancellationToken = default)
        {
            await _ingredientService.DeleteItem(id, cancellationToken);
            await _hubContext.Clients.Group(_groupName).SendAsync(EditStateHub.ItemDeletedMsg, _groupName, id, cancellationToken);
        }
    }
}
