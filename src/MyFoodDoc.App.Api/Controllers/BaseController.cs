using Microsoft.AspNetCore.Mvc;
using MyFoodDoc.App.Infrastructure;

namespace MyFoodDoc.App.Api.Controllers
{
    [ApiVersion("1.0")]
    [Route("api/v1.0/[controller]")]
    [ApiController]
    public abstract class BaseController : ControllerBase
    {
        protected string GetUserId()
        {
            return User.GetLoggedInUserId<string>();
        }
    }
}