using Microsoft.EntityFrameworkCore;
using MyFoodDoc.Application.Abstractions;
using MyFoodDoc.CMS.Application.Models;
using MyFoodDoc.CMS.Application.Persistence;
using MyFoodDoc.CMS.Infrastructure.AzureBlob;
using MyFoodDoc.CMS.Infrastructure.Mock;
using System.Collections.Generic;
using System.IO;
using System.Linq;
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

        public async Task<ImageModel> AddItem(ImageModel item, CancellationToken cancellationToken = default)
        {
            var imageEntity = item.ToEntity(); 
            await _context.Images.AddAsync(imageEntity);

            await _context.SaveChangesAsync(cancellationToken);

            return ImageModel.FromEntity(imageEntity);
        }

        public async Task<ImageModel> UploadImage(Stream stream, CancellationToken cancellationToken = default)
        {
            var image = new ImageModel()
            {
                Url = await _imageBlobService.UploadImage(stream, "image/jpeg", null, cancellationToken)
            };

            var imageEntity = image.ToEntity();
            await _context.Images.AddAsync(imageEntity);

            await _context.SaveChangesAsync(cancellationToken);

            return ImageModel.FromEntity(imageEntity);
        }

        public async Task<bool> DeleteItem(int id, CancellationToken cancellationToken = default)
        {
            var imageEntity = await _context.Images.FirstOrDefaultAsync(u => u.Id == id);

            var result = await _imageBlobService.DeleteImage(imageEntity.Url);

            _context.Images.Remove(imageEntity);
            await _context.SaveChangesAsync(cancellationToken);

            return result;
        }

        public async Task<ImageModel> GetItem(int id)
        {
            var imageEntity = await _context.Images.FirstOrDefaultAsync(u => u.Id == id);
            return ImageModel.FromEntity(imageEntity);
        }

        public async Task<IList<ImageModel>> GetItems()
        {
            var imageEntities = await _context.Images.ToListAsync();
            return imageEntities.Select(ImageModel.FromEntity).ToList();
        }

        public async Task<ImageModel> UpdateItem(ImageModel item, CancellationToken cancellationToken = default)
        {
            var imageEntity = await _context.Images.FirstOrDefaultAsync(u => u.Id == item.Id);

            if (imageEntity == null)
                return null;

            var itemEntity = item.ToEntity(); 

            if (itemEntity.Url != imageEntity.Url)
                await _imageBlobService.DeleteImage(imageEntity.Url);

            _context.Entry(imageEntity).CurrentValues.SetValues(itemEntity);

            await _context.SaveChangesAsync(cancellationToken);

            return ImageModel.FromEntity(imageEntity);
        }
    }
}
