using DotNetify.Security;
using Microsoft.AspNetCore.Mvc;
using MyFoodDoc.CMS.Application.Persistence;
using MyFoodDoc.CMS.Models.VM;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace MyFoodDoc.CMS.Controllers
{
    [Route("api/v1/[controller]")]
    [Authorize]
    public class DictionariesController
    {
        private readonly IInsuranceService _insuranceService = null;

        public DictionariesController(IInsuranceService insuranceService)
        {
            this._insuranceService = insuranceService;
        }

        [HttpGet("insurance")]
        public async Task<IList<Insurance>> GetinsuranceList(CancellationToken cancellationToken = default)
        {
            return (await _insuranceService.GetItems(cancellationToken)).Select(Insurance.FromModel).ToList();
        }
    }
}
