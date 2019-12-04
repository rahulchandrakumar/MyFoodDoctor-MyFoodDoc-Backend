using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Net.Http.Headers;
using MyFoodDoc.CMS.Application.Persistence;
using MyFoodDoc.CMS.Payloads;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace MyFoodDoc.CMS.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    [Authorize]
    public class FilesController : ControllerBase
    {
        private readonly IFileService _fileService;
        private readonly IPromotionService _promotionService;

        public FilesController(IFileService fileService, IPromotionService promotionService)
        {
            this._fileService = fileService;
            this._promotionService = promotionService;
        }

        [HttpPost("uploadTemp")]
        public async Task<int?> UploadTempFile([FromForm]FileUploadPayload payload, CancellationToken cancellationToken = default)
        {
            if (payload?.Files == null)
                return null;

            return await _fileService.StoreTempFile(payload.Files.Files.First().OpenReadStream(), TimeSpan.FromSeconds(15), cancellationToken);
        }

        [HttpGet("coupons")]
        public async Task<IActionResult> GetCoupons(int Id, CancellationToken cancellationToken = default)
        {
            return new FileContentResult(await _promotionService.GetCouponsFile(Id, cancellationToken), MediaTypeHeaderValue.Parse("text/plain"));
        }
    }
}