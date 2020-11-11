using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MyFoodDoc.CMS.Application.Persistence;
using MyFoodDoc.CMS.Models.VM;
using MyFoodDoc.CMS.Payloads;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace MyFoodDoc.CMS.Controllers
{
    [Authorize(Roles = "Viewer")]
    [Route("api/v1/[controller]")]
    [ApiController]
    public class PatientsController : ControllerBase
    {
        private readonly IPatientService _patientService;

        public PatientsController(IPatientService patientService)
        {
            this._patientService = patientService;
        }

        // GET: api/v1/Users
        [HttpGet]
        public async Task<object> Get([FromQuery] PatientsGetPayload payload, CancellationToken cancellationToken = default)
        {
            var paginatedItems = await _patientService.GetItems(payload.Take, payload.Skip, payload.Search, cancellationToken);

            return new
            {
                values = paginatedItems.Items.Select(Patient.FromModel),
                total = paginatedItems.TotalCount
            };
        }

        // GET: api/v1/Users/5
        [HttpGet("{id}")]
        public async Task<Patient> Get(string id, CancellationToken cancellationToken = default)
        {
            return Patient.FromModel(await _patientService.GetItem(id, cancellationToken));
        }
    }
}
