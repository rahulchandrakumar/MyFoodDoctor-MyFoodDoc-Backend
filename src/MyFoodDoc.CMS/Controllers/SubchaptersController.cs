using System;
using System.Collections.Generic;
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
    public class SubchaptersController
    {
        private readonly ISubchapterService _subchapterService;
        private readonly IHubContext<EditStateHub> _hubContext;
        private const string _groupName = "subchapters";

        public SubchaptersController(ISubchapterService subchapterService, IHubContext<EditStateHub> hubContext)
        {
            this._subchapterService = subchapterService;
            this._hubContext = hubContext;
        }

        // GET: api/v1/Subchapters
        [HttpGet]
        public async Task<object> Get([FromQuery] SubchaptersGetPayload payload, CancellationToken cancellationToken = default)
        {
            return new
            {
                values = (await _subchapterService.GetItems(payload.ChapterId, payload.Take, payload.Skip, payload.Search, cancellationToken)).Select(Subchapter.FromModel),
                total = await _subchapterService.GetItemsCount(payload.ChapterId, payload.Search, cancellationToken)
            };
        }

        // GET: api/v1/Subchapters/5
        [HttpGet("{id}")]
        public async Task<Subchapter> Get(int id, CancellationToken cancellationToken = default)
        {
            return Subchapter.FromModel(await _subchapterService.GetItem(id, cancellationToken));
        }

        // POST: api/v1/Subchapters
        [HttpPost]
        public async Task Post([FromBody] Subchapter item, CancellationToken cancellationToken = default)
        {
            var model = await _subchapterService.AddItem(item.ToModel(), cancellationToken);
            await _hubContext.Clients.Group(_groupName).SendAsync(EditStateHub.ItemAddedMsg, _groupName, model.Id, cancellationToken);
        }

        // PUT: api/v1/Subchapters
        [HttpPut()]
        public async Task Put([FromBody] Subchapter item, CancellationToken cancellationToken = default)
        {
            await _subchapterService.UpdateItem(item.ToModel(), cancellationToken);
            await _hubContext.Clients.Group(_groupName).SendAsync(EditStateHub.ItemUpdatedMsg, _groupName, item.Id, cancellationToken);
        }

        // DELETE: api/v1/ChSubchaptersapters/5
        [HttpDelete("{id}")]
        public async Task Delete(int id, CancellationToken cancellationToken = default)
        {
            await _subchapterService.DeleteItem(id, cancellationToken);
            await _hubContext.Clients.Group(_groupName).SendAsync(EditStateHub.ItemDeletedMsg, _groupName, id, cancellationToken);
        }
    }
}
