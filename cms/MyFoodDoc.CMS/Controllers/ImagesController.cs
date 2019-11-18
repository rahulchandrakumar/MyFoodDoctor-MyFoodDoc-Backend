using DotNetify.Security;
using Microsoft.AspNetCore.Mvc;
using MyFoodDoc.CMS.Application.Models;
using MyFoodDoc.CMS.Application.Persistence;
using MyFoodDoc.CMS.Models;
using MyFoodDoc.CMS.Payloads;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace MyFoodDoc.CMS.Controllers
{
    [Route("api/v1/[controller]")]
    [Authorize]
    public class ImagesController
    {
        private readonly IImageService _imageService = null;

        public ImagesController(IImageService imageService)
        {
            this._imageService = imageService;
        }

        [HttpPost("upload")]
        public async Task<Image> UploadImage([FromForm]ImageUploadPayload payload)
        {
            if (payload?.Files == null)
                return null;

            using (var stream = new MemoryStream())
            {
                await payload.Files.Files.First().CopyToAsync(stream);
                var image = Image.FromModel(await _imageService.AddItem(new ImageModel() { ImageData = stream.ToArray() }));

                return image;
            }
        }
    }
}
