using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MyFoodDoc.CMS.Application.Persistence;
using MyFoodDoc.CMS.Payloads;

namespace MyFoodDoc.CMS.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class FilesController : ControllerBase
    {
        private readonly IFileService _fileService = null;

        public FilesController(IFileService fileService)
        {
            this._fileService = fileService;
        }

        [HttpPost("uploadTemp")]
        public async Task<int?> UploadTempFile([FromForm]FileUploadPayload payload, CancellationToken cancellationToken = default)
        {
            if (payload?.Files == null)
                return null;

            return await _fileService.StoreTempFile(payload.Files.Files.First().OpenReadStream(), TimeSpan.FromSeconds(15), cancellationToken);
        }
    }
}