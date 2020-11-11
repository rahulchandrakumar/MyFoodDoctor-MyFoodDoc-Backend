using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using MyFoodDoc.Application.Abstractions;
using MyFoodDoc.Application.Entities.Courses;
using MyFoodDoc.CMS.Application.Models;
using MyFoodDoc.CMS.Application.Persistence;
using MyFoodDoc.CMS.Application.Persistence.Base;
using MyFoodDoc.CMS.Infrastructure.AzureBlob;

namespace MyFoodDoc.CMS.Infrastructure.Persistence
{
    public class ChapterService : IChapterService
    {
        private readonly IApplicationContext _context;
        private readonly IImageBlobService _imageService;
        private readonly ISubchapterService _subchapterService;

        public ChapterService(IApplicationContext context, IImageBlobService imageService, ISubchapterService subchapterService)
        {
            this._context = context;
            this._imageService = imageService;
            this._subchapterService = subchapterService;
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
                .Include(x=> x.Subchapters)
                .FirstOrDefaultAsync(u => u.Id == id, cancellationToken);

            foreach (var subchapter in entity.Subchapters.ToList())
                await _subchapterService.DeleteItem(subchapter.Id, cancellationToken);

            _context.Chapters.Remove(entity);

            if (entity.Image != null)
            {
                _context.Images.Remove(entity.Image);

                await _imageService.DeleteImage(entity.Image.Url, cancellationToken);
            }

            await _context.SaveChangesAsync(cancellationToken);

            return true;
        }

        public async Task<ChapterModel> GetItem(int id, CancellationToken cancellationToken = default)
        {
            var entity = await _context.Chapters
                .Include(x => x.Image)
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.Id == id, cancellationToken);

            return ChapterModel.FromEntity(entity);
        }

        public IQueryable<Chapter> GetBaseQuery(int parentId, string search)
        {
            IQueryable<Chapter> baseQuery = _context.Chapters
                .Include(x => x.Image)
                .Where(x => x.CourseId == parentId);

            if (!string.IsNullOrWhiteSpace(search))
            {
                var searchString = $"%{search}%";
                baseQuery = baseQuery.Where(f => EF.Functions.Like(f.Title, searchString) || EF.Functions.Like(f.Text, searchString));
            }
            return baseQuery;
        }

        public async Task<PaginatedItems<ChapterModel>> GetItems(int parentId, int take, int skip, string search, CancellationToken cancellationToken = default)
        {
            var entities = await GetBaseQuery(parentId, search).AsNoTracking().ToListAsync(cancellationToken);

            return new PaginatedItems<ChapterModel>()
            {
                Items = entities.Skip(skip).Take(take).Select(ChapterModel.FromEntity).OrderBy(x => x.Order).ToList(),
                TotalCount = entities.Count
            };
        }

        public async Task<ChapterModel> UpdateItem(ChapterModel item, CancellationToken cancellationToken = default)
        {
            var entity = await _context.Chapters.FindAsync(new object[] { item.Id }, cancellationToken);

            var oldImageId = entity.ImageId;

            _context.Entry(entity).CurrentValues.SetValues(item.ToEntity());

            if (item.Image.Id != oldImageId)
            {
                var image = await _context.Images.SingleAsync(x => x.Id == oldImageId, cancellationToken);
                _context.Images.Remove(image);

                await _imageService.DeleteImage(image.Url, cancellationToken);
            }

            await _context.SaveChangesAsync(cancellationToken);

            return await GetItem(entity.Id, cancellationToken);
        }
    }
}
