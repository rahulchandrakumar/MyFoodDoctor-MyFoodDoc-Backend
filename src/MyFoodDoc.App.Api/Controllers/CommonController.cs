//using MyFoodDoc.Shared.MailSender;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using MyFoodDoc.App.Application.Helpers;
using MyFoodDoc.App.Application.Payloads.User;
using MyFoodDoc.Application.Api.Helpers;
using System.Net.Mime;
using System.Threading.Tasks;

namespace MyFoodDoc.App.Api.Controllers
{
    public class CommonController : BaseController
    {
        private readonly IIdentityServerClient _identityServerclient;
        private readonly ILogger<UserController> _logger;

        public CommonController(IIdentityServerClient identityServerClient, ILogger<UserController> logger)
        {
            _identityServerclient = identityServerClient;
            _logger = logger;
        }

        [HttpPost("register")]
        [Consumes(MediaTypeNames.Application.Json)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Register([FromBody] RegistrationPayload payload) 
        {
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
        public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordPayload payload)
        {
            return Accepted();
        }
    }
}
