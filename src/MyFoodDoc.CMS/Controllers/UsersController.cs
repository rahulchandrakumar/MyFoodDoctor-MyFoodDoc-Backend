using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using MyFoodDoc.CMS.Application.Common;
using MyFoodDoc.CMS.Application.Persistence;
using MyFoodDoc.CMS.Hubs;
using MyFoodDoc.CMS.Models.VM;
using MyFoodDoc.CMS.Payloads;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace MyFoodDoc.CMS.Controllers
{
    [Authorize(Roles = "Admin")]
    [Route("api/v1/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IHashingManager _hashingManager;
        private readonly IUserService _userService;
        private readonly IHubContext<EditStateHub> _hubContext;
        private const string _groupName = "users";

        public UsersController(IHashingManager hashingManager, IUserService userService, IHubContext<EditStateHub> hubContext)
        {
            this._hashingManager = hashingManager;
            this._userService = userService;
            this._hubContext = hubContext;
        }

        // GET: api/v1/Users
        [HttpGet]
        public async Task<object> Get([FromQuery] UsersGetPayload payload, CancellationToken cancellationToken = default)
        {
            var paginatedItems = await _userService.GetItems(payload.Take, payload.Skip, payload.Search, cancellationToken);

            return new
            {
                values = paginatedItems.Items.Select(Models.VM.User.FromModel),
                total = paginatedItems.TotalCount
            };
        }

        // GET: api/v1/Users/5
        [HttpGet("{id}")]
        public async Task<User> Get(int id, CancellationToken cancellationToken = default)
        {
            return Models.VM.User.FromModel(await _userService.GetItem(id, cancellationToken));
        }

        // POST: api/v1/Users
        [HttpPost]
        public async Task Post([FromBody] User item, CancellationToken cancellationToken = default)
        {
            var model = await _userService.AddItem(item.ToModel(_hashingManager), cancellationToken);
            await _hubContext.Clients.Group(_groupName).SendAsync(EditStateHub.ItemAddedMsg, _groupName, model.Id, cancellationToken);
        }

        // PUT: api/v1/Users
        [HttpPut()]
        public async Task Put([FromBody] User item, CancellationToken cancellationToken = default)
        {
            await _userService.UpdateItem(item.ToModel(_hashingManager), cancellationToken);
            await _hubContext.Clients.Group(_groupName).SendAsync(EditStateHub.ItemUpdatedMsg, _groupName, item.Id, cancellationToken);
        }

        // DELETE: api/v1/Users/5
        [HttpDelete("{id}")]
        public async Task Delete(int id, CancellationToken cancellationToken = default)
        {
            await _userService.DeleteItem(id, cancellationToken);
            await _hubContext.Clients.Group(_groupName).SendAsync(EditStateHub.ItemDeletedMsg, _groupName, id, cancellationToken);
        }
    }
}
