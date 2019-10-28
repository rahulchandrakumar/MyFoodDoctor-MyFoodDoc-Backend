using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MyFoodDoc.App.Application.Payloads.Coupon;
using System.Net.Mime;
using System.Threading.Tasks;

namespace MyFoodDoc.App.Api.Controllers
{
    [Authorize]
    public class CouponController : BaseController
    {
        [HttpPost("redeem")]
        [Consumes(MediaTypeNames.Application.Json)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Redeem([FromBody] CouponPayload payload)
        {
            return Ok();
        }
    }
}