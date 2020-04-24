using Microsoft.EntityFrameworkCore;
using MyFoodDoc.Application.Abstractions;
using MyFoodDoc.Application.Entites.Targets;
using MyFoodDoc.CMS.Application.Models;
using MyFoodDoc.CMS.Application.Persistence;
using MyFoodDoc.CMS.Infrastructure.AzureBlob;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace MyFoodDoc.CMS.Infrastructure.Persistence
{
    public class TargetService : ITargetService
    {
        private readonly IApplicationContext _context;
        private readonly IImageBlobService _imageService;

        public TargetService(IApplicationContext context, IImageBlobService imageService)
        {
            this._context = context;
            this._imageService = imageService;
        }

        public async Task<TargetModel> AddItem(TargetModel item, CancellationToken cancellationToken = default)
        {
            var targetEntity = item.ToEntity();
            await _context.Targets.AddAsync(targetEntity, cancellationToken);

            await _context.SaveChangesAsync(cancellationToken);

            targetEntity = await _context.Targets
                                            .Include(x => x.Image)
                                            .FirstOrDefaultAsync(u => u.Id == targetEntity.Id, cancellationToken);

            return TargetModel.FromEntity(targetEntity);
        }

        public async Task<bool> DeleteItem(int id, CancellationToken cancellationToken = default)
        {
            var targetEntity = await _context.Targets
                                                .Include(x => x.Image)
                                                .FirstOrDefaultAsync(u => u.Id == id, cancellationToken);

            await _imageService.DeleteImage(targetEntity.Image.Url, cancellationToken);

            _context.Images.Remove(targetEntity.Image);
            _context.Targets.Remove(targetEntity);
            await _context.SaveChangesAsync(cancellationToken);

            return true;
        }

        public async Task<TargetModel> GetItem(int id, CancellationToken cancellationToken = default)
        {
            var targetEntity = await _context.Targets
                                            .Include(x => x.Image)
                                            .FirstOrDefaultAsync(x => x.Id == id, cancellationToken);

            return TargetModel.FromEntity(targetEntity);
        }

        public async Task<IList<TargetModel>> GetItems(CancellationToken cancellationToken = default)
        {
            var targetEntities = await _context.Targets
                                                .Include(x => x.Image)
                                                .ToListAsync(cancellationToken);

            return targetEntities.Select(TargetModel.FromEntity).ToList();
        }

        public IQueryable<Target> GetBaseQuery(string search)
        {
            IQueryable<Target> baseQuery = _context.Targets;
            if (!string.IsNullOrWhiteSpace(search))
            {
                var searchstring = $"%{search}%";
                baseQuery = baseQuery.Where(f => EF.Functions.Like(f.Title, searchstring) || EF.Functions.Like(f.Text, searchstring));
            }
            return baseQuery;
        }

        public async Task<IList<TargetModel>> GetItems(int take, int skip, string search, CancellationToken cancellationToken = default)
        {
            var targetEntities = await GetBaseQuery(search)
                                                .Include(x => x.Image)
                                                .Skip(skip).Take(take)
                                                .ToListAsync(cancellationToken);

            return targetEntities.Select(TargetModel.FromEntity).ToList();
        }

        public async Task<long> GetItemsCount(string search, CancellationToken cancellationToken = default)
        {
            return await GetBaseQuery(search).CountAsync(cancellationToken);
        }

        public async Task<TargetModel> UpdateItem(TargetModel item, CancellationToken cancellationToken = default)
        {
            var targetEntity = await _context.Targets.FindAsync(new object[] { item.Id }, cancellationToken);

            _context.Entry(targetEntity).CurrentValues.SetValues(item.ToEntity());

            await _context.SaveChangesAsync(cancellationToken);

            return await GetItem(targetEntity.Id, cancellationToken);
        }
    }
}
