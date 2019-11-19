using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using MyFoodDoc.App.Application.Abstractions;
using MyFoodDoc.App.Application.Models;

namespace MyFoodDoc.App.Api.Controllers
{
    [Authorize]
    public class LexiconController : BaseController
    {
        private readonly ILexiconService _service;
        private readonly ILogger _logger;

        public LexiconController(ILexiconService service, ILogger<LexiconController> logger)
        {
            _service = service;
            _logger = logger;
        }

        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<LexiconShallowEntryDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> List(CancellationToken cancellationToken = default)
        {
            var result = await _service.GetAllAsync(cancellationToken);

            return Ok(result);
        }

        [HttpGet("{id}")]
        [ProducesResponseType(typeof(LexiconEntryDto), StatusCodes.Status200OK)]
        public async Task<IActionResult> List([FromRoute] int id, CancellationToken cancellationToken = default)
        {
            var result = await _service.GetAsync(id, cancellationToken);

            return Ok(result);
        }
    }
}