using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MyFoodDoc.Api.Models;
using MyFoodDoc.App.Application.Mock;

namespace MyFoodDoc.App.Api.Controllers
{
    [Authorize]
    public class FoodController : BaseController
    {
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<Ingredient>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Search(string query)
        {
            var result = IngredientsMock.Entries.Where(x => CultureInfo.InvariantCulture.CompareInfo.IndexOf(x.Name, query, CompareOptions.IgnoreCase) >= 0);
            return Ok(result);
        }
    }
}