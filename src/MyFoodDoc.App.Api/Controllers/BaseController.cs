using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace MyFoodDoc.App.Api.Controllers
{
    [ApiVersion("1.0")]
    [Route("api/v1.0/[controller]")]
    [ApiController]
    public abstract class BaseController : ControllerBase
    {
        
    }
}