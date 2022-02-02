using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using MyFoodDoc.App.Application.Abstractions;
using MyFoodDoc.App.Application.Models;
using MyFoodDoc.App.Application.Payloads.Course;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace MyFoodDoc.App.Api.Controllers
{
    [Authorize]
    public class CoursesController : BaseController
    {
        private readonly ILogger _logger;
        private readonly ICourseService _service;

        public CoursesController(ICourseService service, ILogger<CoursesController> logger)
        {
            _service = service;
            _logger = logger;
        }

        [HttpGet]
        [ProducesResponseType(typeof(ICollection<CourseDto>), StatusCodes.Status200OK)]
        public async Task<ActionResult<ICollection<CourseDto>>> Get(CancellationToken cancellationToken = default)
        {
            var result = await _service.GetAsync(GetUserId(), cancellationToken);

            return Ok(result);
        }

        [HttpGet("{id}")]
        [ProducesResponseType(typeof(CourseDetailsDto), StatusCodes.Status200OK)]
        public async Task<ActionResult<CourseDetailsDto>> GetDetails([FromRoute(Name = "id")] int courseId, CancellationToken cancellationToken = default)
        {
            var result = await _service.GetDetailsAsync(GetUserId(), courseId, cancellationToken);

            return Ok(result);
        }

        [HttpPost("{id}/chapter")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> InsertAnswer([FromRoute(Name = "id")] int courseId, [FromBody] AnswerPayload payload, CancellationToken cancellationToken = default)
        {
            await _service.InsertAnswerAsync(GetUserId(), courseId, payload, cancellationToken);

            return Ok();
        }
    }
}
