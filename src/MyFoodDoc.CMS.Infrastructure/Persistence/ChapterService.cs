using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using MyFoodDoc.Application.Abstractions;
using MyFoodDoc.Application.Entites.Courses;
using MyFoodDoc.CMS.Application.Models;
using MyFoodDoc.CMS.Application.Persistence;
using MyFoodDoc.CMS.Infrastructure.AzureBlob;

namespace MyFoodDoc.CMS.Infrastructure.Persistence
{
    public class ChapterService : IChapterService
    {
        private readonly IApplicationContext _context;
        private readonly IImageBlobService _imageService;

        public ChapterService(IApplicationContext context, IImageBlobService imageService)
        {
            this._context = context;
            this._imageService = imageService;
        }

        public async Task<ChapterModel> AddItem(ChapterModel item, CancellationToken cancellationToken = default)
        {
            var entity = item.ToEntity();
            await _context.Chapters.AddAsync(entity, cancellationToken);

            await _context.SaveChangesAsync(cancellationToken);

            entity = await _context.Chapters
                .Include(x => x.Image)
                .FirstOrDefaultAsync(u => u.Id == entity.Id, cancellationToken);

            return ChapterModel.FromEntity(entity);
        }

        public async Task<bool> DeleteItem(int id, CancellationToken cancellationToken = default)
        {
            var entity = await _context.Chapters
                .Include(x => x.Image)
                .FirstOrDefaultAsync(u => u.Id == id, cancellationToken);

            await _imageService.DeleteImage(entity.Image.Url, cancellationToken);

            _context.Images.Remove(entity.Image);
            _context.Chapters.Remove(entity);
            await _context.SaveChangesAsync(cancellationToken);

            return true;
        }

        public async Task<ChapterModel> GetItem(int id, CancellationToken cancellationToken = default)
        {
            var entity = await _context.Chapters
                .Include(x => x.Image)
                .FirstOrDefaultAsync(x => x.Id == id, cancellationToken);

            return ChapterModel.FromEntity(entity);
        }

        public IQueryable<Chapter> GetBaseQuery(int parentId, string search)
        {
            IQueryable<Chapter> baseQuery = _context.Chapters.Where(x => x.CourseId == parentId);
            if (!string.IsNullOrWhiteSpace(search))
            {
                var searchString = $"%{search}%";
                baseQuery = baseQuery.Where(f => EF.Functions.Like(f.Title, searchString) || EF.Functions.Like(f.Text, searchString));
            }
            return baseQuery;
        }

        public async Task<IList<ChapterModel>> GetItems(int parentId, int take, int skip, string search, CancellationToken cancellationToken = default)
        {
            var entities = await GetBaseQuery(parentId, search)
                .Include(x => x.Image)
                .Skip(skip).Take(take)
                .ToListAsync(cancellationToken);

            return entities.Select(ChapterModel.FromEntity).ToList();
        }

        public async Task<long> GetItemsCount(int parentId, string search, CancellationToken cancellationToken = default)
        {
            return await GetBaseQuery(parentId, search).CountAsync(cancellationToken);
        }

        public async Task<ChapterModel> UpdateItem(ChapterModel item, CancellationToken cancellationToken = default)
        {
            var entity = await _context.Chapters.FindAsync(new object[] { item.Id }, cancellationToken);

            _context.Entry(entity).CurrentValues.SetValues(item.ToEntity());

            await _context.SaveChangesAsync(cancellationToken);

            return await GetItem(entity.Id, cancellationToken);
        }
    }
}
