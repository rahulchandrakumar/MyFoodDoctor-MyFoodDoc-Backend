using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using MyFoodDoc.Application.Abstractions;
using MyFoodDoc.Application.Entities;
using MyFoodDoc.CMS.Application.Models;
using MyFoodDoc.CMS.Application.Persistence;
using MyFoodDoc.CMS.Infrastructure.AzureBlob;

namespace MyFoodDoc.CMS.Infrastructure.Persistence
{
    public class LexiconCategoryService : ILexiconCategoryService
    {
        private readonly IApplicationContext _context;
        private readonly IImageBlobService _imageService;
        private readonly ILexiconService _lexiconService;

        public LexiconCategoryService(IApplicationContext context, IImageBlobService imageService, ILexiconService lexiconService)
        {
            this._context = context;
            this._imageService = imageService;
            this._lexiconService = lexiconService;
        }

        public async Task<LexiconCategoryModel> AddItem(LexiconCategoryModel item, CancellationToken cancellationToken = default)
        {
            var entity = item.ToEntity();
            await _context.LexiconCategories.AddAsync(entity, cancellationToken);

            await _context.SaveChangesAsync(cancellationToken);

            entity = await _context.LexiconCategories
                .Include(x => x.Image)
                .FirstOrDefaultAsync(u => u.Id == entity.Id, cancellationToken);

            return LexiconCategoryModel.FromEntity(entity);
        }

        public async Task<bool> DeleteItem(int id, CancellationToken cancellationToken = default)
        {
            var entity = await _context.LexiconCategories
                .Include(x => x.Image)
                .Include(x => x.Entries)
                .FirstOrDefaultAsync(u => u.Id == id, cancellationToken);

            foreach (var entry in entity.Entries.ToList())
                await _lexiconService.DeleteItem(entry.Id, cancellationToken);

            _context.LexiconCategories.Remove(entity);

            _context.Images.Remove(entity.Image);

            await _imageService.DeleteImage(entity.Image.Url, cancellationToken);

            await _context.SaveChangesAsync(cancellationToken);

            return true;
        }

        public async Task<LexiconCategoryModel> GetItem(int id, CancellationToken cancellationToken = default)
        {
            var course = await _context.LexiconCategories
                .Include(x => x.Image)
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.Id == id, cancellationToken);

            return LexiconCategoryModel.FromEntity(course);
        }

        public IQueryable<LexiconCategory> GetBaseQuery(string search)
        {
            IQueryable<LexiconCategory> baseQuery = _context.LexiconCategories;
            if (!string.IsNullOrWhiteSpace(search))
            {
                var searchString = $"%{search}%";
                baseQuery = baseQuery.Where(f => EF.Functions.Like(f.Title, searchString));
            }
            return baseQuery;
        }

        public async Task<IList<LexiconCategoryModel>> GetItems(int take, int skip, string search, CancellationToken cancellationToken = default)
        {
            var entities = await GetBaseQuery(search)
                .Include(x => x.Image)
                .Skip(skip).Take(take)
                .AsNoTracking()
                .ToListAsync(cancellationToken);

            return entities.Select(x => LexiconCategoryModel.FromEntity(x)).ToList();
        }

        public async Task<long> GetItemsCount(string search, CancellationToken cancellationToken = default)
        {
            return await GetBaseQuery(search).AsNoTracking().CountAsync(cancellationToken);
        }

        public async Task<LexiconCategoryModel> UpdateItem(LexiconCategoryModel item, CancellationToken cancellationToken = default)
        {
            var entity = await _context.LexiconCategories.FindAsync(new object[] { item.Id }, cancellationToken);

            var oldImageId = entity.ImageId;

            _context.Entry(entity).CurrentValues.SetValues(item.ToEntity());

            if (item.Image.Id != oldImageId)
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
