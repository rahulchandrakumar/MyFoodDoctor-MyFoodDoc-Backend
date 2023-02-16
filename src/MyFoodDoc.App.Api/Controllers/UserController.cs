//using MyFoodDoc.Shared.MailSender;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using MyFoodDoc.App.Application.Abstractions;
using MyFoodDoc.App.Application.Models;
using MyFoodDoc.App.Application.Payloads.Diary;
using MyFoodDoc.App.Application.Payloads.User;
using System;
using System.Collections.Generic;
using System.Net.Mime;
using System.Threading;
using System.Threading.Tasks;

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

        [HttpDelete]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> Delete(CancellationToken cancellationToken = default)
        {
            await _service.DeleteUserAsync(GetUserId(), cancellationToken);

            return Ok();
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
            await _service.ChangePasswordAsync(GetUserId(), payload.OldPassword, payload.NewPassword);

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
            await _historyService.UpsertAbdominalGirthHistoryAsync(GetUserId(), payload, cancellationToken);

            var result = await _historyService.GetAbdominalGirthHistoryAsync(GetUserId(), cancellationToken);

            return Ok(result);
        }

        [HttpGet("statistics")]
        [ProducesResponseType(typeof(UserStatisticsDto), StatusCodes.Status200OK)]
        public async Task<ActionResult<UserStatisticsDto>> UserStatisticsReady(CancellationToken cancellationToken = default)
        {
            var userId = GetUserId();

            var user = await _service.GetUserAsync(userId, cancellationToken);

            var result = new UserStatisticsDto
            {
                IsZPPForbidden = await _diaryService.IsZPPForbidden(userId, DateTime.Now, cancellationToken),
                HasSubscription = user.HasSubscription,
                HasZPPSubscription = user.HasZPPSubscription,
                IsDiaryFull = await _diaryService.IsDiaryFull(userId, DateTime.Today.AddMinutes(-1), cancellationToken),
                HasNewTargetsTriggered = await _targetService.NewTriggered(userId, DateTime.Today.AddMinutes(-1), cancellationToken),
                IsFirstTargetsEvaluation = !(await _targetService.AnyAnswered(userId, cancellationToken)),
                HasTargetsActivated = await _targetService.AnyActivated(userId, DateTime.Today.AddMinutes(-1), cancellationToken),
                DaysTillFirstEvaluation = await _targetService.GetDaysTillFirstEvaluationAsync(userId, cancellationToken)
            };

            return Ok(result);
        }

        [HttpPost("notifications")]
        [Consumes(MediaTypeNames.Application.Json)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> UpdatePushNotifications([FromBody] UpdatePushNotificationsPayload payload, CancellationToken cancellationToken = default)
        {
            await _service.UpdatePushNotifications(GetUserId(), payload, cancellationToken);

            return Ok();
        }

        [HttpPost("in-app-purchases/app-store/validate")]
        [Consumes(MediaTypeNames.Application.Json)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        [ProducesResponseType(typeof(InAppPurchaseReceiptValidationResultDto), StatusCodes.Status200OK)]
        public async Task<ActionResult<InAppPurchaseReceiptValidationResultDto>> ValidateAppStoreInAppPurchase([FromBody] ValidateAppStoreInAppPurchasePayload payload, CancellationToken cancellationToken = default)
        {
            var result = new InAppPurchaseReceiptValidationResultDto
            {
                IsValid = await _service.ValidateAppStoreInAppPurchase(GetUserId(), payload, cancellationToken)
            };

            return Ok(result);
        }

        [HttpPost("in-app-purchases/google-play-store/validate")]
        [Consumes(MediaTypeNames.Application.Json)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        [ProducesResponseType(typeof(InAppPurchaseReceiptValidationResultDto), StatusCodes.Status200OK)]
        public async Task<ActionResult<InAppPurchaseReceiptValidationResultDto>> ValidateGooglePlayStoreInAppPurchase([FromBody] ValidateGooglePlayStoreInAppPurchasePayload payload, CancellationToken cancellationToken = default)
        {
            var result = new InAppPurchaseReceiptValidationResultDto
            {
                IsValid = await _service.ValidateGooglePlayStoreInAppPurchase(GetUserId(), payload, cancellationToken)
            };

            return Ok(result);
        }

        [HttpPost("zpp/app-store/validate")]
        [Consumes(MediaTypeNames.Application.Json)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        [ProducesResponseType(typeof(InAppPurchaseReceiptValidationResultDto), StatusCodes.Status200OK)]
        public async Task<ActionResult<InAppPurchaseReceiptValidationResultDto>> ValidateAppStoreZPPSubscription([FromBody] ValidateAppStoreInAppPurchasePayload payload, CancellationToken cancellationToken = default)
        {
            var result = new InAppPurchaseReceiptValidationResultDto
            {
                IsValid = await _service.ValidateAppStoreZPPSubscription(GetUserId(), payload, cancellationToken)
            };

            return Ok(result);
        }

        [HttpPost("zpp/google-play-store/validate")]
        [Consumes(MediaTypeNames.Application.Json)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        [ProducesResponseType(typeof(InAppPurchaseReceiptValidationResultDto), StatusCodes.Status200OK)]
        public async Task<ActionResult<InAppPurchaseReceiptValidationResultDto>> ValidateGooglePlayStoreZPPSubscription([FromBody] ValidateGooglePlayStoreInAppPurchasePayload payload, CancellationToken cancellationToken = default)
        {
            var result = new InAppPurchaseReceiptValidationResultDto
            {
                IsValid = await _service.ValidateGooglePlayStoreZPPSubscription(GetUserId(), payload, cancellationToken)
            };

            return Ok(result);
        }
    }
}
