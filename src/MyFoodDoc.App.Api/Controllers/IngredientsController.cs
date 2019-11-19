using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MyFoodDoc.App.Application.Models;
using MyFoodDoc.App.Application.Mock;
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

        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<IngredientDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Search(string query, CancellationToken cancellationToken)
        {
            var results = await _service.GetAllAsync(query, cancellationToken);

            return Ok(results);
        }
    }
}