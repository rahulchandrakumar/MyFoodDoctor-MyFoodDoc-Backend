using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using MyFoodDoc.App.Application.Abstractions;
using MyFoodDoc.App.Application.Models;
using MyFoodDoc.App.Application.Payloads.Psychogramm;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace MyFoodDoc.App.Api.Controllers
{
    [Authorize]
    public class PsychogrammController : BaseController
    {
        private readonly ILogger _logger;
        private readonly IPsychogrammService _service;

        public PsychogrammController(IPsychogrammService service, ILogger<PsychogrammController> logger)
        {
            _service = service;
            _logger = logger;
        }

        [HttpGet]
        [ProducesResponseType(typeof(ICollection<ScaleDto>), StatusCodes.Status200OK)]
        public async Task<ActionResult<ICollection<ScaleDto>>> Get(CancellationToken cancellationToken = default)
        {
            var result = await _service.GetAsync(GetUserId(), cancellationToken);

            return Ok(result);
        }

        [HttpPost("choices")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> InsertChoices([FromBody] InsertChoicesPayload payload, CancellationToken cancellationToken = default)
        {
            await _service.InsertChoices(GetUserId(), payload, cancellationToken);

            return Ok();
        }

        [HttpGet("evaluation")]
        [ProducesResponseType(typeof(PsychogrammEvaluationResultDto), StatusCodes.Status200OK)]
        public async Task<ActionResult<PsychogrammEvaluationResultDto>> GetEvaluation(CancellationToken cancellationToken = default)
        {
            var result = await _service.GetEvaluationAsync(GetUserId(), cancellationToken);

            return Ok(result);
        }
    }
}
