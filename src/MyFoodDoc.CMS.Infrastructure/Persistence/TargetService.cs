using Microsoft.EntityFrameworkCore;
using MyFoodDoc.Application.Abstractions;
using MyFoodDoc.Application.Entites.Targets;
using MyFoodDoc.Application.Enums;
using MyFoodDoc.CMS.Application.Models;
using MyFoodDoc.CMS.Application.Persistence;
using MyFoodDoc.CMS.Infrastructure.AzureBlob;
using System;
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
            var targetEntity = item.ToTargetEntity();
            await _context.Targets.AddAsync(targetEntity, cancellationToken);

            var adjustmentTargetEntity = item.ToAdjustmentTargetEntity();

            if (adjustmentTargetEntity != null)
                await _context.AdjustmentTargets.AddAsync(adjustmentTargetEntity, cancellationToken);

            await _context.SaveChangesAsync(cancellationToken);

            targetEntity = await _context.Targets
                                            .Include(x => x.Image)
                                            .FirstOrDefaultAsync(u => u.Id == targetEntity.Id, cancellationToken);

            return TargetModel.FromEntity(targetEntity, adjustmentTargetEntity);
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

            AdjustmentTarget adjustmentTargetEntity = null;

            if (targetEntity.Type == TargetType.Adjustment)
                adjustmentTargetEntity = await _context.AdjustmentTargets
                                            .FirstOrDefaultAsync(x => x.Id == id, cancellationToken);

            return TargetModel.FromEntity(targetEntity, adjustmentTargetEntity);
        }

        public async Task<IList<TargetModel>> GetItems(CancellationToken cancellationToken = default)
        {
            var targetEntities = await _context.Targets
                                                .Include(x => x.Image)
                                                .ToListAsync(cancellationToken);

            var adjustmentTargetEntities = await _context.AdjustmentTargets
                                                .ToListAsync(cancellationToken);

            return targetEntities.Select(x => TargetModel.FromEntity(x, adjustmentTargetEntities.SingleOrDefault(y => y.TargetId == x.Id))).ToList();
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

            var adjustmentTargetEntities = await _context.AdjustmentTargets
                                                .ToListAsync(cancellationToken);

            return targetEntities.Select(x => TargetModel.FromEntity(x, adjustmentTargetEntities.SingleOrDefault(y => y.TargetId == x.Id))).ToList();
        }

        public async Task<long> GetItemsCount(string search, CancellationToken cancellationToken = default)
        {
            return await GetBaseQuery(search).CountAsync(cancellationToken);
        }

        public async Task<TargetModel> UpdateItem(TargetModel item, CancellationToken cancellationToken = default)
        {
            try
            {
                var targetEntity = await _context.Targets.FindAsync(new object[] { item.Id }, cancellationToken);

                _context.Entry(targetEntity).CurrentValues.SetValues(item.ToTargetEntity());

                if (targetEntity.Type == TargetType.Adjustment)
                {
                    var adjustmentTargetEntity = await _context.AdjustmentTargets.FindAsync(new object[] { item.AdjustmentTargetId }, cancellationToken);

                    _context.Entry(adjustmentTargetEntity).CurrentValues.SetValues(item.ToAdjustmentTargetEntity());
                }

                await _context.SaveChangesAsync(cancellationToken);

                return await GetItem(targetEntity.Id, cancellationToken);
            }
            catch (Exception e)
            {
                string s = e.Message;
                throw;
            }
        }
    }
}
