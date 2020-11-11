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
    public class ChaptersController : ControllerBase
    {
        private readonly IChapterService _chapterService;
        private readonly IHubContext<EditStateHub> _hubContext;
        private const string _groupName = "chapters";

        public ChaptersController(IChapterService chapterService, IHubContext<EditStateHub> hubContext)
        {
            this._chapterService = chapterService;
            this._hubContext = hubContext;
        }

        // GET: api/v1/Chapters
        [HttpGet]
        public async Task<object> Get([FromQuery] ChaptersGetPayload payload, CancellationToken cancellationToken = default)
        {
            var paginatedItems = await _chapterService.GetItems(payload.CourseId, payload.Take, payload.Skip,
                payload.Search, cancellationToken);

            return new
            {
                values = paginatedItems.Items.Select(Chapter.FromModel),
                total = paginatedItems.TotalCount
            };
        }

        // GET: api/v1/Chapters/5
        [HttpGet("{id}")]
        public async Task<Chapter> Get(int id, CancellationToken cancellationToken = default)
        {
            return Chapter.FromModel(await _chapterService.GetItem(id, cancellationToken));
        }

        // POST: api/v1/Chapters
        [HttpPost]
        public async Task Post([FromBody] Chapter item, CancellationToken cancellationToken = default)
        {
            var model = await _chapterService.AddItem(item.ToModel(), cancellationToken);
            await _hubContext.Clients.Group(_groupName).SendAsync(EditStateHub.ItemAddedMsg, _groupName, model.Id, cancellationToken);
        }

        // PUT: api/v1/Chapters
        [HttpPut()]
        public async Task Put([FromBody] Chapter item, CancellationToken cancellationToken = default)
        {
            await _chapterService.UpdateItem(item.ToModel(), cancellationToken);
            await _hubContext.Clients.Group(_groupName).SendAsync(EditStateHub.ItemUpdatedMsg, _groupName, item.Id, cancellationToken);
        }

        // DELETE: api/v1/Chapters/5
        [HttpDelete("{id}")]
        public async Task Delete(int id, CancellationToken cancellationToken = default)
        {
            await _chapterService.DeleteItem(id, cancellationToken);
            await _hubContext.Clients.Group(_groupName).SendAsync(EditStateHub.ItemDeletedMsg, _groupName, id, cancellationToken);
        }
    }
}
