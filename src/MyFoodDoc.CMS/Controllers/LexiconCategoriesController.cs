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
    public class LexiconCategoriesController : ControllerBase
    {
        private readonly ILexiconCategoryService _lexiconCategoryService;
        private readonly IHubContext<EditStateHub> _hubContext;
        private const string _groupName = "lexiconcategories";

        public LexiconCategoriesController(ILexiconCategoryService lexiconCategoryService, IHubContext<EditStateHub> hubContext)
        {
            this._lexiconCategoryService = lexiconCategoryService;
            this._hubContext = hubContext;
        }

        // GET: api/v1/LexiconCategories
        [HttpGet]
        public async Task<object> Get([FromQuery] CoursesGetPayload payload, CancellationToken cancellationToken = default)
        {
            return new
            {
                values = (await _lexiconCategoryService.GetItems(payload.Take, payload.Skip, payload.Search, cancellationToken)).Select(LexiconCategory.FromModel),
                total = await _lexiconCategoryService.GetItemsCount(payload.Search, cancellationToken)
            };
        }

        // GET: api/v1/LexiconCategories/5
        [HttpGet("{id}")]
        public async Task<LexiconCategory> Get(int id, CancellationToken cancellationToken = default)
        {
            return LexiconCategory.FromModel(await _lexiconCategoryService.GetItem(id, cancellationToken));
        }

        // POST: api/v1/LexiconCategories
        [HttpPost]
        public async Task Post([FromBody] LexiconCategory item, CancellationToken cancellationToken = default)
        {
            var model = await _lexiconCategoryService.AddItem(item.ToModel(), cancellationToken);
            await _hubContext.Clients.Group(_groupName).SendAsync(EditStateHub.ItemAddedMsg, _groupName, model.Id, cancellationToken);
        }

        // PUT: api/v1/LexiconCategories
        [HttpPut()]
        public async Task Put([FromBody] LexiconCategory item, CancellationToken cancellationToken = default)
        {
            await _lexiconCategoryService.UpdateItem(item.ToModel(), cancellationToken);
            await _hubContext.Clients.Group(_groupName).SendAsync(EditStateHub.ItemUpdatedMsg, _groupName, item.Id, cancellationToken);
        }

        // DELETE: api/v1/LexiconCategories/5
        [HttpDelete("{id}")]
        public async Task Delete(int id, CancellationToken cancellationToken = default)
        {
            await _lexiconCategoryService.DeleteItem(id, cancellationToken);
            await _hubContext.Clients.Group(_groupName).SendAsync(EditStateHub.ItemDeletedMsg, _groupName, id, cancellationToken);
        }
    }
}
