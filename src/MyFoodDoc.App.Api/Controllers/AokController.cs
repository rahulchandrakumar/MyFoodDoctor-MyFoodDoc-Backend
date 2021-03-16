using Microsoft.AspNetCore.Authorization;
using MyFoodDoc.AokClient.Abstractions;
using MyFoodDoc.AokClient.Clients;
using Microsoft.AspNetCore.Mvc;
using MyFoodDoc.App.Application.Abstractions;
using MyFoodDoc.App.Application.Payloads.Aok;
using System.Threading.Tasks;
using System.Threading;

namespace MyFoodDoc.App.Api.Controllers
{
    [Authorize]
    public class AokController : BaseController
    {
        private readonly IAokClient _aokClient;
        private readonly IAokService _aokService;

        public AokController(IAokClient aokClient, IAokService aokService)
        {
            _aokClient = aokClient;
            _aokService = aokService;
        }

        [HttpPost("authenticate")]
        public async Task<IActionResult> AuthenticateAsync([FromBody] AuthenticatePayload authenticatePayload, CancellationToken cancellationToken = default)
        {
            var userId = GetUserId();

            if(await _aokService.ExistsAsync(userId))
            {
                return Ok();
            }

            var response = await _aokClient.ValidateAsync(authenticatePayload.InsuranceNumber, authenticatePayload.Birthday);
            if(string.IsNullOrEmpty(response.RegistrationToken))
            {
                var regResponse = await _aokClient.Register(new RegisterRequest
                {
                    InsuranceNumber = authenticatePayload.InsuranceNumber,
                    Birthday = authenticatePayload.Birthday
                });

                response.RegistrationToken = regResponse.RegistrationToken;
            }

            await _aokService.InsertUserAsync(userId, new AokUserPayload
            {
                Token = response.RegistrationToken
            }, cancellationToken);

            return Ok();
        }
    }
}
