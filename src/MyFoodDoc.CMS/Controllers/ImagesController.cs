using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MyFoodDoc.CMS.Application.Persistence;
using MyFoodDoc.CMS.Models.VMBase;
using MyFoodDoc.CMS.Payloads;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace MyFoodDoc.CMS.Controllers
{
    [Route("api/v1/[controller]")]
    [Authorize]
    public class ImagesController : ControllerBase
    {
        private readonly IImageService _imageService = null;

        public ImagesController(IImageService imageService)
        {
            this._imageService = imageService;
        }

        [HttpPost("upload")]
        public async Task<Image> UploadImage([FromForm] FileUploadPayload payload, CancellationToken cancellationToken = default)
        {
            if (payload?.Files == null)
                return null;

            var file = payload.Files.Files.First();

            var image = Image.FromModel(await _imageService.UploadImage(file.OpenReadStream(), file.ContentType, cancellationToken));

            return image;
        }
    }
}
