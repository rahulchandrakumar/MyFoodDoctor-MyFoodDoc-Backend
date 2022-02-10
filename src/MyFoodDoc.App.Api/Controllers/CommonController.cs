//using MyFoodDoc.Shared.MailSender;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using MyFoodDoc.App.Application.Abstractions;
using MyFoodDoc.App.Application.Clients;
using MyFoodDoc.App.Application.Exceptions;
using MyFoodDoc.App.Application.Payloads.User;
using MyFoodDoc.Application.Abstractions;
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

            var emailBody = "Hallo,<br/><br/>" +
                            "Herzlich Willkommen bei myFoodDoctor, – dem erfolgreichsten digitalen Ernährungscoach für unterwegs & zuhause!<br/><br/>" +
                            "Wie schön, dass du deine Gesundheit und Ernährung mit der myFoodDoctor-App selbst in die Hand nehmen möchtest – ganz egal, welches persönliche Ziel du auch verfolgst: ob du abnehmen, die Symptome deiner Erkrankung lindern oder dich einfach rundum gesund und fit fühlen willst.<br/><br/>" +
                            "Lass' es uns gemeinsam angehen – aber ganz in deinem Tempo.<br/><br/>" +
                            "Wir stehen dir mit professionellem Rat und Tat zur Seite.<br/><br/>" +
                            "Für Rückfragen wende dich an support@myfooddoctor.de. Wir helfen dir gerne!<br/><br/>" +
                            "Herzliche Grüße<br/><br/>" + "Dr. Matthias Riedl und das myFoodDoctor-Team";

            var result = await _emailService.SendEmailAsync(payload.Email, null, "Willkommen bei myFoodDoctor – deiner Ernährungscoaching-App", emailBody);

            if (!result)
            {
                _logger.LogError($"Unable to send a welcome email to {payload.Email}");
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

            var emailBody = "Hallo,<br/><br/>" +
                            "hier ist die angeforderte PIN für die Bestätigung deines neuen Passwortes.<br/><br/>" +
                            $"{token}<br/><br/>" + "Die PIN ist 5 Minuten lang gültig.<br/><br/>" +
                            "Wenn es nicht geklappt hat, ist entweder die PIN abgelaufen oder die Eingabe war fehlerhaft. Dann kannst du einfach eine neue PIN beantragen und es noch einmal versuchen.<br/><br/>" +
                            "Oder wende dich im Zweifelsfall an support@myfooddoctor.de. Wir helfen dir gerne weiter!<br/><br/>" +
                            "Sollte diese Anfrage nicht von dir kommen, melde uns dies bitte zum Schutz deiner Daten.<br/><br/>" +
                            "Herzliche Grüße,<br/><br/>" + "dein myFoodDoctor-Team";

            var result = await _emailService.SendEmailAsync(tokenPayload.Email, null, "myFoodDoctor - Passwort zurücksetzen", emailBody);

            if (!result)
            {
                throw new BadRequestException($"Unable to send an email to {tokenPayload.Email}");
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
