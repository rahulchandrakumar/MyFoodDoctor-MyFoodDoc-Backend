using Microsoft.EntityFrameworkCore;
using MyFoodDoc.Application.Abstractions;
using MyFoodDoc.Application.Entities.Targets;
using MyFoodDoc.Application.Enums;
using MyFoodDoc.CMS.Application.Models;
using MyFoodDoc.CMS.Application.Persistence;
using MyFoodDoc.CMS.Infrastructure.AzureBlob;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MyFoodDoc.CMS.Application.Persistence.Base;

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
            //Target
            var targetEntity = item.ToTargetEntity();

            await _context.Targets.AddAsync(targetEntity, cancellationToken);

            await _context.SaveChangesAsync(cancellationToken);

            //AdjustmentTarget
            var adjustmentTargetEntity = item.ToAdjustmentTargetEntity();

            if (adjustmentTargetEntity != null)
            {
                adjustmentTargetEntity.TargetId = targetEntity.Id;

                await _context.AdjustmentTargets.AddAsync(adjustmentTargetEntity, cancellationToken);

                await _context.SaveChangesAsync(cancellationToken);
            }

            item.Id = targetEntity.Id;

            //DietTargets
            var targetDiets = item.ToDietTargetEntities();

            if (targetDiets != null)
            {
                await _context.DietTargets.AddRangeAsync(targetDiets, cancellationToken);

                await _context.SaveChangesAsync(cancellationToken);
            }

            //IndicationTargets
            var targetIndications = item.ToIndicationTargetEntities();

            if (targetIndications != null)
            {
                await _context.IndicationTargets.AddRangeAsync(targetIndications, cancellationToken);

                await _context.SaveChangesAsync(cancellationToken);
            }

            //MotivationTargets
            var targetMotivations = item.ToMotivationTargetEntities();

            if (targetMotivations != null)
            {
                await _context.MotivationTargets.AddRangeAsync(targetMotivations, cancellationToken);

                await _context.SaveChangesAsync(cancellationToken);
            }

            targetEntity = await _context.Targets
                                            .Include(x => x.Image)
                                            .Include(x => x.Diets)
                                            .Include(x => x.Indications)
                                            .Include(x => x.Motivations)
                                            .FirstOrDefaultAsync(u => u.Id == targetEntity.Id, cancellationToken);

            return TargetModel.FromEntity(targetEntity, adjustmentTargetEntity);
        }

        public async Task<bool> DeleteItem(int id, CancellationToken cancellationToken = default)
        {
            var targetEntity = await _context.Targets
                                                .Include(x => x.Image)
                                                .FirstOrDefaultAsync(u => u.Id == id, cancellationToken);
            
            _context.Targets.Remove(targetEntity);
            
            _context.Images.Remove(targetEntity.Image);

            await _imageService.DeleteImage(targetEntity.Image.Url, cancellationToken);
            
            await _context.SaveChangesAsync(cancellationToken);

            return true;
        }

        public async Task<TargetModel> GetItem(int id, CancellationToken cancellationToken = default)
        {
            var targetEntity = await _context.Targets
                                            .Include(x => x.Image)
                                            .Include(x => x.Diets)
                                            .Include(x => x.Indications)
                                            .Include(x => x.Motivations)
                                            .AsNoTracking()
                                            .FirstOrDefaultAsync(x => x.Id == id, cancellationToken);

            AdjustmentTarget adjustmentTargetEntity = null;

            if (targetEntity.Type == TargetType.Adjustment)
                adjustmentTargetEntity = await _context.AdjustmentTargets
                                            .FirstOrDefaultAsync(x => x.TargetId == id, cancellationToken);

            return TargetModel.FromEntity(targetEntity, adjustmentTargetEntity);
        }

        public async Task<IList<TargetModel>> GetItems(CancellationToken cancellationToken = default)
        {
            var targetEntities = await _context.Targets
                                                .Include(x => x.Image)
                                                .Include(x => x.Diets)
                                                .Include(x => x.Indications)
                                                .Include(x => x.Motivations)
                                                .AsNoTracking()
                                                .ToListAsync(cancellationToken);

            var adjustmentTargetEntities = await _context.AdjustmentTargets
                                                .ToListAsync(cancellationToken);

            return targetEntities.Select(x => TargetModel.FromEntity(x, adjustmentTargetEntities.SingleOrDefault(y => y.TargetId == x.Id))).ToList();
        }

        public IQueryable<Target> GetBaseQuery(int parentId, string search)
        {
            IQueryable<Target> baseQuery = _context.Targets
                .Include(x => x.Image)
                .Include(x => x.Diets)
                .Include(x => x.Indications)
                .Include(x => x.Motivations)
                .Where(x => x.OptimizationAreaId == parentId);

            if (!string.IsNullOrWhiteSpace(search))
            {
                var searchstring = $"%{search}%";
                baseQuery = baseQuery.Where(f => EF.Functions.Like(f.Title, searchstring) || EF.Functions.Like(f.Text, searchstring));
            }
            return baseQuery;
        }

        public async Task<PaginatedItems<TargetModel>> GetItems(int parentId, int take, int skip, string search, CancellationToken cancellationToken = default)
        {
            var targetEntities = await GetBaseQuery(parentId, search).AsNoTracking().ToListAsync(cancellationToken);

            var adjustmentTargetEntities = await _context.AdjustmentTargets
                .ToListAsync(cancellationToken);

            return new PaginatedItems<TargetModel>()
            {
                Items = targetEntities.Skip(skip).Take(take).Select(x => TargetModel.FromEntity(x, adjustmentTargetEntities.SingleOrDefault(y => y.TargetId == x.Id))).ToList(),
                TotalCount = targetEntities.Count
            };
        }

        public async Task<TargetModel> UpdateItem(TargetModel item, CancellationToken cancellationToken = default)
        {
            //Target
            var targetEntity = await _context.Targets.FindAsync(new object[] { item.Id }, cancellationToken);

            var oldImageId = targetEntity.ImageId;

            var newTargetEntity = item.ToTargetEntity();

            _context.Entry(targetEntity).CurrentValues.SetValues(newTargetEntity);

            //Image
            if (item.Image.Id != oldImageId)
            {
                var oldImage = await _context.Images.SingleAsync(x => x.Id == oldImageId, cancellationToken);
                _context.Images.Remove(oldImage);

                await _imageService.DeleteImage(oldImage.Url, cancellationToken);
            }

            //AdjustmentTarget
            if (targetEntity.Type == TargetType.Adjustment)
            {
                if (newTargetEntity.Type == TargetType.Adjustment)
                {
                    var adjustmentTargetEntity =
                        await _context.AdjustmentTargets.FindAsync(new object[] { item.AdjustmentTargetId },
                            cancellationToken);

                    _context.Entry(adjustmentTargetEntity).CurrentValues.SetValues(item.ToAdjustmentTargetEntity());
                }
                else
                {
                    var adjustmentTargetEntity = await _context.AdjustmentTargets.FirstOrDefaultAsync(x => x.Id == item.AdjustmentTargetId, cancellationToken);

                    _context.AdjustmentTargets.Remove(adjustmentTargetEntity);
                }
            }
            else if (newTargetEntity.Type == TargetType.Adjustment)
            {
                var adjustmentTargetEntity = item.ToAdjustmentTargetEntity();

                await _context.AdjustmentTargets.AddAsync(adjustmentTargetEntity, cancellationToken);
            }

            await _context.SaveChangesAsync(cancellationToken);

            //DietTargets
            var existingTargetDiets = await _context.DietTargets.Where(x => x.TargetId == item.Id).ToListAsync(cancellationToken);

            _context.DietTargets.RemoveRange(existingTargetDiets);

            await _context.SaveChangesAsync(cancellationToken);

            var targetDiets = item.ToDietTargetEntities();

            if (targetDiets != null)
            {
                await _context.DietTargets.AddRangeAsync(targetDiets, cancellationToken);

                await _context.SaveChangesAsync(cancellationToken);
            }

            //IndicationTargets
            var existingTargetIndications = await _context.IndicationTargets.Where(x => x.TargetId == item.Id).ToListAsync(cancellationToken);

            _context.IndicationTargets.RemoveRange(existingTargetIndications);

            await _context.SaveChangesAsync(cancellationToken);

            var targetIndications = item.ToIndicationTargetEntities();

            if (targetIndications != null)
            {
                await _context.IndicationTargets.AddRangeAsync(targetIndications, cancellationToken);

                await _context.SaveChangesAsync(cancellationToken);
            }

            //MotivationTargets
            var existingTargetMotivations = await _context.MotivationTargets.Where(x => x.TargetId == item.Id).ToListAsync(cancellationToken);

            _context.MotivationTargets.RemoveRange(existingTargetMotivations);

            await _context.SaveChangesAsync(cancellationToken);
            
            var targetMotivations = item.ToMotivationTargetEntities();

            if (targetMotivations != null)
            {
                await _context.MotivationTargets.AddRangeAsync(targetMotivations, cancellationToken);

                await _context.SaveChangesAsync(cancellationToken);
            }

            return await GetItem(targetEntity.Id, cancellationToken);
        }
    }
}
