//using MyFoodDoc.Shared.MailSender;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using MyFoodDoc.App.Application.Abstractions;
using MyFoodDoc.App.Application.Clients;
using MyFoodDoc.App.Application.Payloads.User;
using System;
using System.Net.Mime;
using System.Threading;
using System.Threading.Tasks;
using MyFoodDoc.App.Application.Exceptions;
using MyFoodDoc.Application.Abstractions;

namespace MyFoodDoc.App.Api.Controllers
{
    [AllowAnonymous]
    public class CommonController : BaseController
    {
        private readonly ICommonService _service;
        private readonly ILogger _logger;
        private readonly IIdentityServerClient _identityServerClient;
        private readonly IEmailService _emailService;

        public CommonController(ICommonService service, 
            ILogger<CommonController> logger, 
            IIdentityServerClient identityServerClient, 
            IEmailService emailService)
        {
            _service = service;
            _logger = logger;
            _identityServerClient = identityServerClient;
            _emailService = emailService;
        }

        [HttpPost("register")]
        [Consumes(MediaTypeNames.Application.Json)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        public async Task<IActionResult> Register([FromBody] RegisterPayload payload, CancellationToken cancellationToken = default) 
        {
            await _service.RegisterAsync(payload.Email, payload.Password, payload.InsuranceId);

            var response = await _identityServerClient.RequestPasswordTokenAsync(payload.Email, payload.Password);

            if (response.IsError)
            {
                _logger.LogError(response.Error);
                return BadRequest();
            }

            return Content(response.Raw, MediaTypeNames.Application.Json);
        }

        [HttpPost("password/forgot")]
        [Consumes(MediaTypeNames.Application.Json)]
        [ProducesResponseType(StatusCodes.Status202Accepted)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> ForgotPassword([FromBody] ForgotPasswordPayload tokenPayload, CancellationToken cancellationToken = default)
        {
            string token = await _service.GeneratePasswordResetTokenAsync(tokenPayload.Email);

            var result = await _emailService.SendEmailAsync(tokenPayload.Email, "Reset password pin", token);

            if (!result)
            {
                throw new BadRequestException("Unable to send email");
            }

            return Accepted();
        }

        [HttpPost("password/reset")]
        [Consumes(MediaTypeNames.Application.Json)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordPayload payload, CancellationToken cancellationToken = default)
        {
            await _service.ResetPasswordAsync(payload.Email, payload.ResetToken, payload.NewPassword);

            return NoContent();
        }
    }
}
