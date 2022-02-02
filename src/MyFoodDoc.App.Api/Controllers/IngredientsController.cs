
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using MyFoodDoc.App.Application.Abstractions;
using MyFoodDoc.App.Application.Models;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace MyFoodDoc.App.Api.Controllers
{
    [Authorize]
    public class IngredientsController : BaseController
    {
        private readonly IFoodService _service;
        private readonly ILogger _logger;

        public IngredientsController(IFoodService service, ILogger<IngredientsController> logger)
        {
            _service = service;
            _logger = logger;
        }

        [HttpGet("{foodId}")]
        [ProducesResponseType(typeof(ICollection<IngredientDto>), StatusCodes.Status200OK)]
        public async Task<ActionResult<ICollection<IngredientDto>>> Get([FromRoute] long foodId, CancellationToken cancellationToken = default)
        {
            var result = await _service.GetFoodAsync(foodId, cancellationToken);

            return Ok(result);
        }
    }
}