//using MyFoodDoc.Shared.MailSender;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using MyFoodDoc.App.Application.Models;
using MyFoodDoc.App.Application.Mock;
using MyFoodDoc.App.Application.Payloads.User;
using System.Net.Mime;
using System.Threading.Tasks;
using MyFoodDoc.App.Application.Abstractions;
using System.Threading;

namespace MyFoodDoc.App.Api.Controllers
{
    [Authorize]
    public partial class UserController : BaseController
    {
        private IUserService _service;
        private readonly ILogger _logger;

        public UserController(IUserService service, ILogger<UserController> logger)
        {
            _service = service;
            _logger = logger;
        }

        [HttpGet]
        [ProducesResponseType(typeof(UserDto), StatusCodes.Status200OK)]
        public async Task<IActionResult> Current(CancellationToken cancellationToken = default)
        {
            var user = await _service.GetUserAsync(GetUserId(), cancellationToken);

            return Ok(user);
        }

        [HttpPut]
        [Consumes(MediaTypeNames.Application.Json)]
        [ProducesResponseType(typeof(UserDto), StatusCodes.Status200OK)]
        public async Task<IActionResult> Update([FromBody] UpdateUserPayload payload, CancellationToken cancellationToken = default)
        {
            await _service.UpdateUserAsync(GetUserId(), payload, cancellationToken);

            var result = await _service.GetUserAsync(GetUserId(), cancellationToken);

            return Ok(result);
        }

        [HttpPost("anamnesis")]
        [Consumes(MediaTypeNames.Application.Json)]
        [ProducesResponseType(typeof(UserDto), StatusCodes.Status200OK)]
        public async Task<IActionResult> CompleteAnamnesis([FromBody] AnamnesisPayload payload, CancellationToken cancellationToken = default)
        {
            await _service.StoreAnamnesisAsync(GetUserId(), payload, cancellationToken);

            var result = await _service.GetUserAsync(GetUserId(), cancellationToken);

            return Ok(result);
        }

        [HttpPut("password")]
        [Consumes(MediaTypeNames.Application.Json)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordPayload payload, CancellationToken cancellationToken = default)
        {
            await _service.ChangePassword(GetUserId(), payload.OldPassword, payload.NewPassword);

            return NoContent();
        }
    }
}
