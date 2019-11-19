using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MyFoodDoc.CMS.Auth;
using MyFoodDoc.CMS.Payloads;

namespace MyFoodDoc.CMS.Controllers
{
    [Route("api/v1/[controller]")]
    [Authorize]
    public class AuthController: Controller
    {
        private readonly ICustomAuthenticationService _authService;

        public AuthController(ICustomAuthenticationService authService)
        {
            _authService = authService;
        }

        /// <summary>
        /// Login
        /// </summary>
        /// <param name="request">Username/Password/Remember Me</param>
        /// <returns>User info (name, roles)</returns>
        [HttpPost("login")]
        [AllowAnonymous]
        public ActionResult Login([FromBody]LoginPayload request)
        {
            request.Password = request.Password.Trim();
            request.Username = request.Username.Trim();

            var user = _authService.Login(request.Username, request.Password);
            if (user != null)
            {
                return Ok(user);
            }

            return BadRequest();
        }
    }
}
