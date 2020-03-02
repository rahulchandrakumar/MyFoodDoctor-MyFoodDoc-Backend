
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

        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(IngredientDto), StatusCodes.Status200OK)]
        public async Task<ActionResult<IngredientDto>> Get([FromRoute] int id, CancellationToken cancellationToken = default)
        {
            var result = await _service.GetAsync(id, cancellationToken);

            return Ok(result);
        }
    }
}