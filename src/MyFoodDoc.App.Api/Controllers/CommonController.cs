//using MyFoodDoc.Shared.MailSender;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using MyFoodDoc.App.Application.Abstractions;
using MyFoodDoc.App.Application.Helpers;
using MyFoodDoc.App.Application.Payloads.User;
using MyFoodDoc.Application.Api.Helpers;
using System;
using System.Net.Mime;
using System.Threading;
using System.Threading.Tasks;

namespace MyFoodDoc.App.Api.Controllers
{
    [AllowAnonymous]
    public class CommonController : BaseController
    {
        private readonly ICommonService _service;
        private readonly ILogger _logger;
        private readonly IIdentityServerClient _identityServerclient;

        public CommonController(ICommonService service, ILogger<CommonController> logger, IIdentityServerClient identityServerClient)
        {
            _service = service;
            _logger = logger;
            _identityServerclient = identityServerClient;
        }

        [HttpPost("register")]
        [Consumes(MediaTypeNames.Application.Json)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Register([FromBody] RegisterPayload payload, CancellationToken cancellationToken = default) 
        {
            await _service.RegisterAsync(payload, cancellationToken);

            var response = await _identityServerclient.RequestPasswordTokenAsync(payload.Email, payload.Password);

            if (response.IsError)
            {
                _logger.LogError(response.Error);
                return BadRequest();
            }

            return Content(response.Raw, MediaTypeNames.Application.Json);
        }

        [HttpPost("password/reset")]
        [Consumes(MediaTypeNames.Application.Json)]
        [ProducesResponseType(StatusCodes.Status202Accepted)]
        public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordPayload payload, CancellationToken cancellationToken = default)
        {
            return Accepted();
        }
    }
}
