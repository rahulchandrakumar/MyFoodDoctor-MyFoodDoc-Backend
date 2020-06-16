using Microsoft.AspNetCore.Authorization;
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
        private readonly IDietService _dietService;
        private readonly IIndicationService _indicationService;
        private readonly IInsuranceService _insuranceService;
        private readonly IMotivationService _motivationService;

        public DictionariesController(IDietService dietService, IIndicationService indicationService, IInsuranceService insuranceService, IMotivationService motivationService)
        {
            this._dietService = dietService;
            this._indicationService = indicationService;
            this._insuranceService = insuranceService;
            this._motivationService = motivationService;
        }

        [HttpGet("diet")]
        public async Task<IList<Diet>> GetDietList(CancellationToken cancellationToken = default)
        {
            return (await _dietService.GetItems(cancellationToken)).Select(Diet.FromModel).ToList();
        }

        [HttpGet("indication")]
        public async Task<IList<Indication>> GetIndicationList(CancellationToken cancellationToken = default)
        {
            return (await _indicationService.GetItems(cancellationToken)).Select(Indication.FromModel).ToList();
        }

        [HttpGet("insurance")]
        public async Task<IList<Insurance>> GetInsuranceList(CancellationToken cancellationToken = default)
        {
            return (await _insuranceService.GetItems(cancellationToken)).Select(Insurance.FromModel).ToList();
        }

        [HttpGet("motivation")]
        public async Task<IList<Motivation>> GetMotivationList(CancellationToken cancellationToken = default)
        {
            return (await _motivationService.GetItems(cancellationToken)).Select(Motivation.FromModel).ToList();
        }
    }
}
