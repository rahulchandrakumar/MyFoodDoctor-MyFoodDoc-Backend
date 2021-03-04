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
    public class QuestionsController : ControllerBase
    {
        private readonly IQuestionService _questionService;
        private readonly IHubContext<EditStateHub> _hubContext;
        private const string _groupName = "questions";

        public QuestionsController(IQuestionService questionService, IHubContext<EditStateHub> hubContext)
        {
            this._questionService = questionService;
            this._hubContext = hubContext;
        }

        // GET: api/v1/Questions
        [HttpGet]
        public async Task<object> Get([FromQuery] QuestionsGetPayload payload, CancellationToken cancellationToken = default)
        {
            var paginatedItems = await _questionService.GetItems(payload.ScaleId, payload.Take, payload.Skip,
                payload.Search, cancellationToken);

            return new
            {
                values = paginatedItems.Items.Select(Question.FromModel),
                total = paginatedItems.TotalCount
            };
        }

        // GET: api/v1/Questions/5
        [HttpGet("{id}")]
        public async Task<Question> Get(int id, CancellationToken cancellationToken = default)
        {
            return Question.FromModel(await _questionService.GetItem(id, cancellationToken));
        }

        // POST: api/v1/Questions
        [HttpPost]
        public async Task Post([FromBody] Question item, CancellationToken cancellationToken = default)
        {
            var model = await _questionService.AddItem(item.ToModel(), cancellationToken);
            await _hubContext.Clients.Group(_groupName).SendAsync(EditStateHub.ItemAddedMsg, _groupName, model.Id, cancellationToken);
        }

        // PUT: api/v1/Questions
        [HttpPut()]
        public async Task Put([FromBody] Question item, CancellationToken cancellationToken = default)
        {
            await _questionService.UpdateItem(item.ToModel(), cancellationToken);
            await _hubContext.Clients.Group(_groupName).SendAsync(EditStateHub.ItemUpdatedMsg, _groupName, item.Id, cancellationToken);
        }

        // DELETE: api/v1/Questions/5
        [HttpDelete("{id}")]
        public async Task Delete(int id, CancellationToken cancellationToken = default)
        {
            await _questionService.DeleteItem(id, cancellationToken);
            await _hubContext.Clients.Group(_groupName).SendAsync(EditStateHub.ItemDeletedMsg, _groupName, id, cancellationToken);
        }
    }
}
