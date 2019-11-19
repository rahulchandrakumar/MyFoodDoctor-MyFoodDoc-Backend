using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using MyFoodDoc.App.Application.Abstractions;
using MyFoodDoc.App.Application.Payloads.Coupon;
using System;
using System.Net.Mime;
using System.Threading;
using System.Threading.Tasks;

namespace MyFoodDoc.App.Api.Controllers
{
    [Authorize]
    public class CouponController : BaseController
    {
        private readonly ICouponService _service;
        private readonly ILogger<UserController> _logger;

        public CouponController(ICouponService service, ILogger<UserController> logger)
        {
            _service = service;
            _logger = logger;
        }

        [HttpPost("redeem")]
        [Consumes(MediaTypeNames.Application.Json)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Redeem([FromBody] CouponPayload payload, CancellationToken cancellationToken = default)
        {
            await _service.RedeemCouponAsync(GetUserId(), payload, cancellationToken);
            return Ok();
        }
    }
}