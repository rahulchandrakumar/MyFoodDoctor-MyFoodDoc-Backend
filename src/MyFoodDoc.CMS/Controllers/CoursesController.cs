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
    public class CoursesController : ControllerBase
    {
        private readonly ICourseService _courseService;
        private readonly IHubContext<EditStateHub> _hubContext;
        private const string _groupName = "courses";

        public CoursesController(ICourseService courseService, IHubContext<EditStateHub> hubContext)
        {
            this._courseService = courseService;
            this._hubContext = hubContext;
        }

        // GET: api/v1/Courses
        [HttpGet]
        public async Task<object> Get([FromQuery] CoursesGetPayload payload, CancellationToken cancellationToken = default)
        {
            var paginatedItems = await _courseService.GetItems(payload.Take, payload.Skip, payload.Search, cancellationToken);

            return new
            {
                values = paginatedItems.Items.Select(Course.FromModel),
                total = paginatedItems.TotalCount
            };
        }

        // GET: api/v1/Courses/5
        [HttpGet("{id}")]
        public async Task<Course> Get(int id, CancellationToken cancellationToken = default)
        {
            return Course.FromModel(await _courseService.GetItem(id, cancellationToken));
        }

        // POST: api/v1/Courses
        [HttpPost]
        public async Task Post([FromBody] Course item, CancellationToken cancellationToken = default)
        {
            var model = await _courseService.AddItem(item.ToModel(), cancellationToken);
            await _hubContext.Clients.Group(_groupName).SendAsync(EditStateHub.ItemAddedMsg, _groupName, model.Id, cancellationToken);
        }

        // PUT: api/v1/Courses
        [HttpPut()]
        public async Task Put([FromBody] Course item, CancellationToken cancellationToken = default)
        {
            await _courseService.UpdateItem(item.ToModel(), cancellationToken);
            await _hubContext.Clients.Group(_groupName).SendAsync(EditStateHub.ItemUpdatedMsg, _groupName, item.Id, cancellationToken);
        }

        // DELETE: api/v1/Courses/5
        [HttpDelete("{id}")]
        public async Task Delete(int id, CancellationToken cancellationToken = default)
        {
            await _courseService.DeleteItem(id, cancellationToken);
            await _hubContext.Clients.Group(_groupName).SendAsync(EditStateHub.ItemDeletedMsg, _groupName, id, cancellationToken);
        }
    }
}
