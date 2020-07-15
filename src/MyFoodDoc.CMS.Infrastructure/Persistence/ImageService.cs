using MyFoodDoc.Application.Abstractions;
using MyFoodDoc.CMS.Application.Models;
using MyFoodDoc.CMS.Application.Persistence;
using MyFoodDoc.CMS.Infrastructure.AzureBlob;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace MyFoodDoc.CMS.Infrastructure.Persistence
{
    public class ImageService : IImageService
    {
        private readonly IApplicationContext _context;
        private readonly IImageBlobService _imageBlobService;
        public ImageService(IApplicationContext context, IImageBlobService imageBlobService)
        {
            this._context = context;
            this._imageBlobService = imageBlobService;
        }

        public async Task<ImageModel> UploadImage(Stream stream, string contentType, CancellationToken cancellationToken = default)
        {
            var image = new ImageModel()
            {
                Url = await _imageBlobService.UploadImage(stream, contentType, null, cancellationToken)
            };

            var imageEntity = image.ToEntity();
            await _context.Images.AddAsync(imageEntity, cancellationToken);

            await _context.SaveChangesAsync(cancellationToken);

            return ImageModel.FromEntity(imageEntity);
        }
    }
}
