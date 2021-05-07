using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MyFoodDoc.App.Application.Models;
using MyFoodDoc.App.Application.Payloads.Diary;
using MyFoodDoc.App.Application.Abstractions;
using Microsoft.Extensions.Logging;
using System.Threading;

namespace MyFoodDoc.App.Api.Controllers
{
    [Authorize]
    public class DiaryController : BaseController
    {
        private readonly IDiaryService _service;
        private readonly ILogger _logger;

        public DiaryController(IDiaryService service, ILogger<DiaryController> logger)
        {
            _service = service;
            _logger = logger;
        }

        [HttpGet("overview/{date:Date}")]
        [ProducesResponseType(typeof(DiaryEntryDto), StatusCodes.Status200OK)]
        public async Task<ActionResult<DiaryEntryDto>> GetByDate([FromRoute] DateTime date, CancellationToken cancellationToken = default)
        {
            var result = await _service.GetAggregationByDateAsync(GetUserId(), date, cancellationToken);

            return Ok(result);
        }

        [HttpPost("meal")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(DiaryEntryDtoMeal), StatusCodes.Status200OK)]
        public async Task<ActionResult<DiaryEntryDtoMeal>> AddMeal([FromBody] InsertMealPayload payload, CancellationToken cancellationToken = default)
        {
            var id = await _service.InsertMealAsync(GetUserId(), payload, cancellationToken);

            var result = await _service.GetMealAsync(GetUserId(), id, cancellationToken);

            return Ok(result);
        }

        [HttpGet("meal/{id}")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(DiaryEntryDtoMeal), StatusCodes.Status200OK)]
        public async Task<ActionResult<DiaryEntryDtoMeal>> GetMeal([FromRoute] int id, CancellationToken cancellationToken = default)
        {
            var result = await _service.GetMealAsync(GetUserId(), id, cancellationToken);

            return Ok(result);
        }

        [HttpPut("meal/{id}")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(DiaryEntryDtoMeal), StatusCodes.Status200OK)]
        public async Task<ActionResult<DiaryEntryDtoMeal>> UpdateMeal([FromRoute] int id, [FromBody] UpdateMealPayload payload, CancellationToken cancellationToken = default)
        {
            await _service.UpdateMealAsync(GetUserId(), id, payload, cancellationToken);

            var result = await _service.GetMealAsync(GetUserId(), id, cancellationToken);

            return Ok(result);
        }

        [HttpDelete("meal/{id}")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> RemoveMeal([FromRoute] int id, CancellationToken cancellationToken = default)
        {
            await _service.RemoveMealAsync(GetUserId(), id, cancellationToken);

            return Ok();
        }

        [HttpPut("liquid")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(DiaryEntryDtoLiquid), StatusCodes.Status200OK)]
        public async Task<ActionResult<DiaryEntryDtoLiquid>> UpdateLiquid([FromBody] LiquidPayload payload, CancellationToken cancellationToken = default)
        {
            await _service.UpsertLiquidAsync(GetUserId(), payload, cancellationToken);

            var result = await _service.GetLiquidAsync(GetUserId(), payload.Date, cancellationToken);

            return Ok(result);
        }

        [HttpPut("exercise")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(DiaryEntryDtoExercise), StatusCodes.Status200OK)]
        public async Task<ActionResult<DiaryEntryDtoExercise>> UpdateExercise([FromBody] ExercisePayload payload, CancellationToken cancellationToken = default)
        {
            await _service.UpsertExerciseAsync(GetUserId(), payload, cancellationToken);

            var result = await _service.GetExerciseAsync(GetUserId(), payload.Date, cancellationToken);

            return Ok(result);
        }

        [HttpGet("favourite")]
        [ProducesResponseType(typeof(ICollection<FavouriteDto>), StatusCodes.Status200OK)]
        public async Task<ActionResult<ICollection<FavouriteDto>>> GetFavourites(CancellationToken cancellationToken = default)
        {
            var result = await _service.GetFavouritesAsync(GetUserId(), cancellationToken);

            return Ok(result);
        }

        [HttpGet("favourite/{id}")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(FavouriteDto), StatusCodes.Status200OK)]
        public async Task<ActionResult<FavouriteDto>> GetFavourite([FromRoute] int id, CancellationToken cancellationToken = default)
        {
            var result = await _service.GetFavouriteAsync(GetUserId(), id, cancellationToken);

            return Ok(result);
        }

        [HttpPost("favourite")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(FavouriteDto), StatusCodes.Status200OK)]
        public async Task<ActionResult<FavouriteDto>> AddFavourite([FromBody] FavouritePayload payload, CancellationToken cancellationToken = default)
        {
            var id = await _service.InsertFavouriteAsync(GetUserId(), payload, true, cancellationToken);

            var result = await _service.GetFavouriteAsync(GetUserId(), id, cancellationToken);

            return Ok(result);
        }

        [HttpPut("favourite/{id}")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(FavouriteDto), StatusCodes.Status200OK)]
        public async Task<ActionResult<FavouriteDto>> UpdateFavourite([FromRoute] int id, [FromBody] FavouritePayload payload, CancellationToken cancellationToken = default)
        {
            await _service.UpdateFavouriteAsync(GetUserId(), id, payload, cancellationToken);

            var result = await _service.GetFavouriteAsync(GetUserId(), id, cancellationToken);

            return Ok(result);
        }

        [HttpDelete("favourite/{id}")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> RemoveFavourite([FromRoute] int id, CancellationToken cancellationToken = default)
        {
            await _service.RemoveFavouriteAsync(GetUserId(), id, cancellationToken);

            return Ok();
        }

        [HttpGet("export")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> Export([FromBody] ExportPayload payload, CancellationToken cancellationToken = default)
        {
            await _service.ExportAsync(GetUserId(), payload, cancellationToken);

            return Ok();
        }
    }
}