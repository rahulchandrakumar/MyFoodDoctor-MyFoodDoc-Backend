//using MyFoodDoc.Shared.MailSender;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using MyFoodDoc.App.Application.Models;
using MyFoodDoc.App.Application.Payloads.User;
using System.Net.Mime;
using System.Threading.Tasks;
using MyFoodDoc.App.Application.Abstractions;
using System.Threading;
using MyFoodDoc.App.Application.Payloads.Diary;
using System.Collections.Generic;

namespace MyFoodDoc.App.Api.Controllers
{
    [Authorize]
    public class UserController : BaseController
    {
        private IUserService _service;
        private IUserHistoryService _historyService;
        private readonly ILogger _logger;

        public UserController(IUserService service, IUserHistoryService historyService, ILogger<UserController> logger)
        {
            _service = service;
            _historyService = historyService;
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

        [HttpGet("history")]
        [Consumes(MediaTypeNames.Application.Json)]
        [ProducesResponseType(typeof(UserHistoryDto), StatusCodes.Status200OK)]
        public async Task<ActionResult<UserHistoryDto>> GetUserHistory(CancellationToken cancellationToken = default)
        {
            var result = await _historyService.GetAggregationAsync(GetUserId(), cancellationToken);

            return Ok(result);
        }

        [HttpPut("history/weight")]
        [Consumes(MediaTypeNames.Application.Json)]
        [ProducesResponseType(typeof(IEnumerable<UserHistoryDtoWeight>), StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<UserHistoryDtoWeight>>> UpdateUserHistoryWeight([FromBody] WeightHistoryPayload payload, CancellationToken cancellationToken = default)
        {
            await _historyService.UpsertWeightHistoryAsync(GetUserId(), payload, cancellationToken);

            var result = await _historyService.GetWeightHistoryAsync(GetUserId(), cancellationToken);

            return Ok(result);
        }

        [HttpPut("history/abdominal-girth")]
        [Consumes(MediaTypeNames.Application.Json)]
        [ProducesResponseType(typeof(IEnumerable<UserHistoryDtoAbdominalGirth>), StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<UserHistoryDtoAbdominalGirth>>> UpdateUserHistoryAbdominalGirth([FromBody] AbdominalGirthHistoryPayload payload, CancellationToken cancellationToken = default)
        {
            await _historyService.UpsertAbdonimalGirthHistoryAsync(GetUserId(), payload, cancellationToken);

            var result = await _historyService.GetAbdonimalGirthHistoryAsync(GetUserId(), cancellationToken);

            return Ok(result);
        }
    }
}
