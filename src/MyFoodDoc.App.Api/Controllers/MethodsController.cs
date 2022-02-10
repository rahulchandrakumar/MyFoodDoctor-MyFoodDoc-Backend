using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using MyFoodDoc.App.Application.Abstractions;
using MyFoodDoc.App.Application.Models;
using MyFoodDoc.App.Application.Payloads.Method;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace MyFoodDoc.App.Api.Controllers
{
    [Authorize]
    public class MethodsController : BaseController
    {
        private readonly ILogger _logger;
        private readonly IMethodService _service;

        public MethodsController(IMethodService service, ILogger<MethodsController> logger)
        {
            _service = service;
            _logger = logger;
        }

        [HttpGet]
        [ProducesResponseType(typeof(ICollection<MethodDto>), StatusCodes.Status200OK)]
        public async Task<ActionResult<ICollection<MethodDto>>> Get(CancellationToken cancellationToken = default)
        {
            var result = await _service.GetAsync(GetUserId(), DateTime.Now, cancellationToken);

            return Ok(result);
        }

        [HttpGet("overview/{date:Date}")]
        [ProducesResponseType(typeof(ICollection<MethodDto>), StatusCodes.Status200OK)]
        public async Task<ActionResult<ICollection<MethodDto>>> GetByDate([FromRoute] DateTime date, CancellationToken cancellationToken = default)
        {
            var result = await _service.GetByDateAsync(GetUserId(), date, cancellationToken);

            return Ok(result);
        }

        [HttpPost("method")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> Insert([FromBody] InsertMethodPayload payload, CancellationToken cancellationToken = default)
        {
            await _service.InsertAsync(GetUserId(), payload, cancellationToken);

            return Ok();
        }
    }
}