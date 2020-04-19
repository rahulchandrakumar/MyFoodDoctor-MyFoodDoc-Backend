
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Threading;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MyFoodDoc.App.Application.Models;
using MyFoodDoc.App.Application.Abstractions;
using Microsoft.Extensions.Logging;
using MyFoodDoc.App.Application.Payloads.Target;

namespace MyFoodDoc.App.Api.Controllers
{
    [Authorize]
    public class TargetsController : BaseController
    {
        private readonly ILogger _logger;
        private readonly ITargetService _service;

        public TargetsController(ITargetService service, ILogger<TargetsController> logger)
        {
            _service = service;
            _logger = logger;
        }

        [HttpGet]
        [ProducesResponseType(typeof(ICollection<OptimizationAreaDto>), StatusCodes.Status200OK)]
        public async Task<ActionResult<ICollection<OptimizationAreaDto>>> Get(CancellationToken cancellationToken = default)
        {
            var result = await _service.GetAsync(GetUserId(), cancellationToken);

            return Ok(result);
        }

        [HttpPost("target")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> Insert([FromBody] InsertTargetPayload payload, CancellationToken cancellationToken = default)
        {
            await _service.InsertAsync(GetUserId(), payload, cancellationToken);

            return Ok();
        }
    }
}
