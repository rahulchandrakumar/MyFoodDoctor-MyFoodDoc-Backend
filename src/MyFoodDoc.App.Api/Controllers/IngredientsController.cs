
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MyFoodDoc.App.Application.Models;
using MyFoodDoc.App.Application.Abstractions;
using Microsoft.Extensions.Logging;
using System.Threading;

namespace MyFoodDoc.App.Api.Controllers
{
    [Authorize]
    public class IngredientsController : BaseController
    {
        private readonly IFoodService _service;
        private readonly ILogger _logger;

        public IngredientsController(IFoodService service, ILogger<CommonController> logger)
        {
            _service = service;
            _logger = logger;
        }

        [HttpGet("{foodId}")]
        [ProducesResponseType(typeof(ICollection<IngredientDto>), StatusCodes.Status200OK)]
        public async Task<ActionResult<ICollection<IngredientDto>>> Get([FromRoute] long foodId, CancellationToken cancellationToken = default)
        {
            var result = await _service.GetAsync(foodId, cancellationToken);

            return Ok(result);
        }
    }
}