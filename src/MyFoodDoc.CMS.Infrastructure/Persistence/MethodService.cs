using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using MyFoodDoc.Application.Abstractions;
using MyFoodDoc.Application.Entities.Methods;
using MyFoodDoc.Application.Enums;
using MyFoodDoc.CMS.Application.Models;
using MyFoodDoc.CMS.Application.Persistence;
using MyFoodDoc.CMS.Infrastructure.AzureBlob;

namespace MyFoodDoc.CMS.Infrastructure.Persistence
{
    public class MethodService : IMethodService
    {
        private readonly IApplicationContext _context;
        private readonly IImageBlobService _imageService;
        private readonly IMethodMultipleChoiceService _methodMultipleChoiceService;

        public MethodService(IApplicationContext context, IImageBlobService imageService, IMethodMultipleChoiceService methodMultipleChoiceService)
        {
            this._context = context;
            this._imageService = imageService;
            this._methodMultipleChoiceService = methodMultipleChoiceService;
        }
        public async Task<MethodModel> AddItem(MethodModel item, CancellationToken cancellationToken = default)
        {
            var entity = item.ToEntity();
            await _context.Methods.AddAsync(entity, cancellationToken);

            await _context.SaveChangesAsync(cancellationToken);

            item.Id = entity.Id;

            //TargetMethods
            var targetMethods = item.ToTargetMethodEntities();

            if (targetMethods != null)
            {
                await _context.TargetMethods.AddRangeAsync(targetMethods, cancellationToken);

                await _context.SaveChangesAsync(cancellationToken);
            }

            //DietMethods
            var methodDiets = item.ToDietMethodEntities();

            if (methodDiets != null)
            {
                await _context.DietMethods.AddRangeAsync(methodDiets, cancellationToken);

                await _context.SaveChangesAsync(cancellationToken);
            }

            //IndicationMethods
            var methodIndications = item.ToIndicationMethodEntities();

            if (methodIndications != null)
            {
                await _context.IndicationMethods.AddRangeAsync(methodIndications, cancellationToken);

                await _context.SaveChangesAsync(cancellationToken);
            }

            //MotivationMethods
            var methodMotivations = item.ToMotivationMethodEntities();

            if (methodMotivations != null)
            {
                await _context.MotivationMethods.AddRangeAsync(methodMotivations, cancellationToken);

                await _context.SaveChangesAsync(cancellationToken);
            }

            entity = await _context.Methods
                .Include(x => x.Image)
                .Include(x => x.Targets)
                .Include(x => x.Diets)
                .Include(x => x.Indications)
                .Include(x => x.Motivations)
                .FirstOrDefaultAsync(u => u.Id == entity.Id, cancellationToken);

            return MethodModel.FromEntity(entity);
        }

        public async Task<bool> DeleteItem(int id, CancellationToken cancellationToken = default)
        {
            var entity = await _context.Methods
                .Include(x => x.Image)
                .FirstOrDefaultAsync(u => u.Id == id, cancellationToken);

            if (entity.Type == MethodType.Knowledge)
                foreach (var methodMultipleChoice in await _context.MethodMultipleChoice.Where(x => x.MethodId == id).ToListAsync(cancellationToken))
                    await _methodMultipleChoiceService.DeleteItem(methodMultipleChoice.Id, cancellationToken);

            _context.Methods.Remove(entity);

            if (entity.Image != null)
            {
                _context.Images.Remove(entity.Image);

                await _imageService.DeleteImage(entity.Image.Url, cancellationToken);
            }

            await _context.SaveChangesAsync(cancellationToken);

            return true;
        }

        public async Task<MethodModel> GetItem(int id, CancellationToken cancellationToken = default)
        {
            var entity = await _context.Methods
                .Include(x => x.Image)
                .Include(x => x.Targets)
                .Include(x => x.Diets)
                .Include(x => x.Indications)
                .Include(x => x.Motivations)
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.Id == id, cancellationToken);

            return MethodModel.FromEntity(entity);
        }

        public async Task<IList<Method>> GetItems(string search, CancellationToken cancellationToken = default)
        {
            var entities = await _context.Methods
                .Include(x => x.Image)
                .Include(x => x.Targets)
                .Include(x => x.Diets)
                .Include(x => x.Indications)
                .Include(x => x.Motivations)
                .AsNoTracking()
                .ToListAsync(cancellationToken);

            if (!string.IsNullOrWhiteSpace(search))
            {
                entities = entities.Where(f => f.Title.Contains(search, StringComparison.InvariantCultureIgnoreCase)
                                               || f.Text.Contains(search, StringComparison.InvariantCultureIgnoreCase)
                                               || f.Type.ToString().Contains(search, StringComparison.InvariantCultureIgnoreCase)).ToList();
            }

            return entities;
        }

        public async Task<IList<MethodModel>> GetItems(int take, int skip, string search, CancellationToken cancellationToken = default)
        {
            var entities = (await GetItems(search, cancellationToken))
                .Skip(skip).Take(take)
                .ToList();

            return entities.Select(MethodModel.FromEntity).ToList();
        }

        public async Task<long> GetItemsCount(string search, CancellationToken cancellationToken = default)
        {
            return (await GetItems(search, cancellationToken)).Count();
        }

        public async Task<MethodModel> UpdateItem(MethodModel item, CancellationToken cancellationToken = default)
        {
            var entity = await _context.Methods.FindAsync(new object[] { item.Id }, cancellationToken);

            var oldImageId = entity.ImageId;

            var newEntity = item.ToEntity();

            //MethodMultipleChoice
            if (newEntity.Type != MethodType.Knowledge && entity.Type == MethodType.Knowledge)
                foreach (var methodMultipleChoice in await _context.MethodMultipleChoice.Where(x => x.MethodId == entity.Id).ToListAsync(cancellationToken))
                    await _methodMultipleChoiceService.DeleteItem(methodMultipleChoice.Id, cancellationToken);

            _context.Entry(entity).CurrentValues.SetValues(newEntity);

            //Image
            if (oldImageId != null && (item.Image == null || string.IsNullOrEmpty(item.Image.Url) || item.Image.Id != oldImageId))
            {
                var image = await _context.Images.SingleAsync(x => x.Id == oldImageId.Value, cancellationToken);
                _context.Images.Remove(image);

                await _imageService.DeleteImage(image.Url, cancellationToken);
            }

            await _context.SaveChangesAsync(cancellationToken);

            //TargetMethods
            var existingTargetMethods = await _context.TargetMethods.Where(x => x.MethodId == item.Id).ToListAsync(cancellationToken);

            _context.TargetMethods.RemoveRange(existingTargetMethods);

            await _context.SaveChangesAsync(cancellationToken);

            var targetMethods = item.ToTargetMethodEntities();

            if (targetMethods != null)
            {
                await _context.TargetMethods.AddRangeAsync(targetMethods, cancellationToken);

                await _context.SaveChangesAsync(cancellationToken);
            }
            
            //DietMethods
            var existingMethodDiets = await _context.DietMethods.Where(x => x.MethodId == item.Id).ToListAsync(cancellationToken);

            _context.DietMethods.RemoveRange(existingMethodDiets);

            await _context.SaveChangesAsync(cancellationToken);

            var methodDiets = item.ToDietMethodEntities();

            if (methodDiets != null)
            {
                await _context.DietMethods.AddRangeAsync(methodDiets, cancellationToken);

                await _context.SaveChangesAsync(cancellationToken);
            }

            //IndicationMethods
            var existingMethodIndications = await _context.IndicationMethods.Where(x => x.MethodId == item.Id).ToListAsync(cancellationToken);

            _context.IndicationMethods.RemoveRange(existingMethodIndications);

            await _context.SaveChangesAsync(cancellationToken);

            var methodIndications = item.ToIndicationMethodEntities();

            if (methodIndications != null)
            {
                await _context.IndicationMethods.AddRangeAsync(methodIndications, cancellationToken);

                await _context.SaveChangesAsync(cancellationToken);
            }

            //MotivationMethods
            var existingMethodMotivations = await _context.MotivationMethods.Where(x => x.MethodId == item.Id).ToListAsync(cancellationToken);

            _context.MotivationMethods.RemoveRange(existingMethodMotivations);

            await _context.SaveChangesAsync(cancellationToken);

            var methodMotivations = item.ToMotivationMethodEntities();

            if (methodMotivations != null)
            {
                await _context.MotivationMethods.AddRangeAsync(methodMotivations, cancellationToken);

                await _context.SaveChangesAsync(cancellationToken);
            }

            return await GetItem(entity.Id, cancellationToken);
        }
    }
}
