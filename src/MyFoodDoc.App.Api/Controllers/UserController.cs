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
        private readonly IUserService _service;
        private readonly IUserHistoryService _historyService;
        private readonly IDiaryService _diaryService;
        private readonly ITargetService _targetService;
        private readonly ILogger _logger;

        public UserController(IUserService service, IUserHistoryService historyService, IDiaryService diaryService, ITargetService targetService, ILogger<UserController> logger)
        {
            _service = service;
            _historyService = historyService;
            _diaryService = diaryService;
            _targetService = targetService;
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

        [HttpGet("statistics")]
        [ProducesResponseType(typeof(UserStatisticsDto), StatusCodes.Status200OK)]
        public async Task<ActionResult<UserStatisticsDto>> UserStatisticsReady(CancellationToken cancellationToken = default)
        {
            var user = GetUserId();

            var result = new UserStatisticsDto
            {
                HasSubscription = (await _service.GetUserAsync(user, cancellationToken)).HasSubscription,
                IsDiaryFull = await _diaryService.IsDiaryFull(user, cancellationToken),
                HasNewTargetsTriggered = await _targetService.NewTriggered(user, cancellationToken),
                IsFirstTargetsEvaluation = !(await _targetService.AnyAnswered(user, cancellationToken))
            };

            return Ok(result);
        }

        [HttpPost("subscription/{hasSubscription}")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> UpdateUserHasSubscription([FromRoute] bool hasSubscription, CancellationToken cancellationToken = default)
        {
            await _service.UpdateUserHasSubscription(GetUserId(), hasSubscription, cancellationToken);

            return Ok();
        }
    }
}
