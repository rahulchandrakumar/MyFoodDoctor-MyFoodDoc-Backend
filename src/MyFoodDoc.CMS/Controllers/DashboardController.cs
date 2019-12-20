using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MyFoodDoc.CMS.Application.Persistence;
using System.Threading;
using System.Threading.Tasks;

namespace MyFoodDoc.CMS.Controllers
{
    [Authorize(Roles = "Viewer")]
    [Route("api/v1/[controller]")]
    [ApiController]
    public class DashboardController : ControllerBase
    {
        private readonly IPatientService _patientService;

        public DashboardController(IPatientService patientService)
        {
            this._patientService = patientService;
        }

        // GET: api/v1/Users
        [HttpGet]
        public async Task<object> Get(CancellationToken cancellationToken = default)
        {
            return new
            {
                fullUserHistory = await _patientService.FullUserHistory(cancellationToken)
            };
        }
    }
}
