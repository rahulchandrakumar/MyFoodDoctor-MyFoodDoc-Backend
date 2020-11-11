using Microsoft.EntityFrameworkCore;
using MyFoodDoc.Application.Abstractions;
using MyFoodDoc.Application.EnumEntities;
using MyFoodDoc.CMS.Application.Models;
using MyFoodDoc.CMS.Application.Persistence;
using MyFoodDoc.CMS.Infrastructure.AzureBlob;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MyFoodDoc.CMS.Application.Persistence.Base;

namespace MyFoodDoc.CMS.Infrastructure.Persistence
{
    public class OptimizationAreaService : IOptimizationAreaService
    {
        private readonly IApplicationContext _context;
        private readonly IImageBlobService _imageService;

        public OptimizationAreaService(IApplicationContext context, IImageBlobService imageService)
        {
            this._context = context;
            this._imageService = imageService;
        }

        public async Task<OptimizationAreaModel> AddItem(OptimizationAreaModel item, CancellationToken cancellationToken = default)
        {
            var entity = item.ToEntity();
            await _context.OptimizationAreas.AddAsync(entity, cancellationToken);

            await _context.SaveChangesAsync(cancellationToken);

            entity = await _context.OptimizationAreas
                                            .Include(x => x.Image)
                                            .FirstOrDefaultAsync(u => u.Id == entity.Id, cancellationToken);

            return OptimizationAreaModel.FromEntity(entity);
        }

        public async Task<bool> DeleteItem(int id, CancellationToken cancellationToken = default)
        {
            var entity = await _context.OptimizationAreas
                                                .Include(x => x.Image)
                                                .FirstOrDefaultAsync(u => u.Id == id, cancellationToken);

            _context.OptimizationAreas.Remove(entity);

            _context.Images.Remove(entity.Image);

            await _imageService.DeleteImage(entity.Image.Url, cancellationToken);

            await _context.SaveChangesAsync(cancellationToken);

            return true;
        }

        public async Task<OptimizationAreaModel> GetItem(int id, CancellationToken cancellationToken = default)
        {
            var entity = await _context.OptimizationAreas
                                            .Include(x => x.Image)
                                            .FirstOrDefaultAsync(x => x.Id == id, cancellationToken);

            return OptimizationAreaModel.FromEntity(entity);
        }

        public IQueryable<OptimizationArea> GetBaseQuery(string search)
        {
            IQueryable<OptimizationArea> baseQuery = _context.OptimizationAreas.Include(x => x.Image);

            if (!string.IsNullOrWhiteSpace(search))
            {
                var searchstring = $"%{search}%";
                baseQuery = baseQuery.Where(f => EF.Functions.Like(f.Name, searchstring) || EF.Functions.Like(f.Text, searchstring));
            }
            return baseQuery;
        }

        public async Task<PaginatedItems<OptimizationAreaModel>> GetItems(int take, int skip, string search, CancellationToken cancellationToken = default)
        {
            var entities = await GetBaseQuery(search).AsNoTracking().ToListAsync(cancellationToken);

            return new PaginatedItems<OptimizationAreaModel>()
            {
                Items = entities.Skip(skip).Take(take).Select(OptimizationAreaModel.FromEntity).ToList(),
                TotalCount = entities.Count
            };
        }

        public async Task<OptimizationAreaModel> UpdateItem(OptimizationAreaModel item, CancellationToken cancellationToken = default)
        {
            var entity = await _context.OptimizationAreas.FindAsync(new object[] { item.Id }, cancellationToken);

            var oldImageId = entity.ImageId;

            _context.Entry(entity).CurrentValues.SetValues(item.ToEntity());

            if (item.Image.Id != oldImageId && oldImageId != null)
            {
                var oldImage = await _context.Images.SingleAsync(x => x.Id == oldImageId, cancellationToken);
                _context.Images.Remove(oldImage);

                await _imageService.DeleteImage(oldImage.Url, cancellationToken);
            }

            await _context.SaveChangesAsync(cancellationToken);

            return await GetItem(entity.Id, cancellationToken);
        }
    }
}
