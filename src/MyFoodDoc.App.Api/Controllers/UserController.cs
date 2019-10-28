//using MyFoodDoc.Shared.MailSender;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using MyFoodDoc.Api.Models;
using MyFoodDoc.App.Application.Mock;
using MyFoodDoc.App.Application.Payloads.User;
using System.Net.Mime;
using System.Threading.Tasks;

namespace MyFoodDoc.App.Api.Controllers
{
    [Authorize]
    public partial class UserController : BaseController
    {
        private readonly ILogger _logger;

        public UserController(ILogger<UserController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        [ProducesResponseType(typeof(User), StatusCodes.Status200OK)]
        public async Task<IActionResult> Current()
        {
            return Ok(UserMock.Default);
        }

        [HttpPut]
        [Consumes(MediaTypeNames.Application.Json)]
        [ProducesResponseType(typeof(User), StatusCodes.Status200OK)]
        public async Task<IActionResult> Update([FromBody] UserPayload payload)
        {
            var newUser = new User
            {
                Email = UserMock.Default.Email,
                Birthday = payload.Birthday,
                Gender = payload.Gender,
                Height = payload.Height,
                InsuranceId = payload.InsuranceId,
                Motivations = payload.Motivations,
                Indications = payload.Indications,
                Diet = payload.Diet,
            };

            return Ok(newUser);
        }

        [HttpPost("anamnesis")]
        [Consumes(MediaTypeNames.Application.Json)]
        [ProducesResponseType(typeof(User), StatusCodes.Status200OK)]
        public async Task<IActionResult> CompleteAnamnesis([FromBody] AnamnesisPayload payload)
        {
            var newUser = new User
            {
                Email = UserMock.Default.Email,
                Gender = payload.Gender,
                Height = payload.Height,
                Motivations = payload.Motivations,
                Indications = payload.Indications,
            };

            return Ok(newUser);
        }

        [HttpPut("password")]
        [Consumes(MediaTypeNames.Application.Json)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordPayload payload)
        {
            return NoContent();
        }
    }
}
