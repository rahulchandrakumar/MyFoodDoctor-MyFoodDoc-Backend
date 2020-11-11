using Microsoft.EntityFrameworkCore;
using MyFoodDoc.Application.Abstractions;
using MyFoodDoc.Application.Entities;
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
    public class LexiconService : ILexiconService
    {
        private readonly IApplicationContext _context;
        private readonly IImageBlobService _imageService;

        public LexiconService(IApplicationContext context, IImageBlobService imageService)
        {
            this._context = context;
            this._imageService = imageService;
        }

        public async Task<LexiconModel> AddItem(LexiconModel item, CancellationToken cancellationToken = default)
        {
            var lexiconEntity = item.ToEntity();
            await _context.LexiconEntries.AddAsync(lexiconEntity, cancellationToken);

            await _context.SaveChangesAsync(cancellationToken);

            lexiconEntity = await _context.LexiconEntries
                                            .Include(x => x.Image)
                                            .FirstOrDefaultAsync(u => u.Id == lexiconEntity.Id, cancellationToken);

            return LexiconModel.FromEntity(lexiconEntity);
        }

        public async Task<bool> DeleteItem(int id, CancellationToken cancellationToken = default)
        {
            var lexiconEntity = await _context.LexiconEntries
                                                .Include(x => x.Image)
                                                .FirstOrDefaultAsync(u => u.Id == id, cancellationToken);

            _context.LexiconEntries.Remove(lexiconEntity);

            _context.Images.Remove(lexiconEntity.Image);

            await _imageService.DeleteImage(lexiconEntity.Image.Url, cancellationToken);

            await _context.SaveChangesAsync(cancellationToken);

            return true;
        }

        public async Task<LexiconModel> GetItem(int id, CancellationToken cancellationToken = default)
        {
            var lexiconEntity = await _context.LexiconEntries
                                            .Include(x => x.Image)
                                            .FirstOrDefaultAsync(x => x.Id == id, cancellationToken);

            return LexiconModel.FromEntity(lexiconEntity);
        }

        public IQueryable<LexiconEntry> GetBaseQuery(int parentId, string search)
        {
            IQueryable<LexiconEntry> baseQuery = _context.LexiconEntries
                .Include(x => x.Image)
                .Where(x => x.CategoryId == parentId);

            if (!string.IsNullOrWhiteSpace(search))
            {
                var searchstring = $"%{search}%";
                baseQuery = baseQuery.Where(f => EF.Functions.Like(f.TitleShort, searchstring) || EF.Functions.Like(f.TitleLong, searchstring));
            }
            return baseQuery;
        }

        public async Task<PaginatedItems<LexiconModel>> GetItems(int parentId, int take, int skip, string search, CancellationToken cancellationToken = default)
        {
            var entities = await GetBaseQuery(parentId, search).AsNoTracking().ToListAsync(cancellationToken);

            return new PaginatedItems<LexiconModel>()
            {
                Items = entities.Skip(skip).Take(take).Select(LexiconModel.FromEntity).ToList(),
                TotalCount = entities.Count
            };
        }

        public async Task<LexiconModel> UpdateItem(LexiconModel item, CancellationToken cancellationToken = default)
        {
            var lexiconEntity = await _context.LexiconEntries.FindAsync(new object[] { item.Id }, cancellationToken);

            var oldImageId = lexiconEntity.ImageId;

            _context.Entry(lexiconEntity).CurrentValues.SetValues(item.ToEntity());

            if (item.Image.Id != oldImageId)
            {
                var oldImage = await _context.Images.SingleAsync(x => x.Id == oldImageId, cancellationToken);
                _context.Images.Remove(oldImage);

                await _imageService.DeleteImage(oldImage.Url, cancellationToken);
            }

            await _context.SaveChangesAsync(cancellationToken);

            return await GetItem(lexiconEntity.Id, cancellationToken);
        }
    }
}
