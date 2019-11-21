using Microsoft.EntityFrameworkCore;
using MyFoodDoc.Application.Abstractions;
using MyFoodDoc.CMS.Application.Models;
using MyFoodDoc.CMS.Application.Persistence;
using MyFoodDoc.CMS.Infrastructure.AzureBlob;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

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
            await _context.LexiconEntries.AddAsync(lexiconEntity);

            await _context.SaveChangesAsync(cancellationToken);

            lexiconEntity = await _context.LexiconEntries
                                            .Include(x => x.Image)
                                            .FirstOrDefaultAsync(u => u.Id == lexiconEntity.Id);

            return LexiconModel.FromEntity(lexiconEntity);
        }

        public async Task<bool> DeleteItem(object id, CancellationToken cancellationToken = default)
        {
            var lexiconEntity = await _context.LexiconEntries.FindAsync(id);
            await _context.Entry(lexiconEntity).Reference(p => p.Image).LoadAsync();

            await _imageService.DeleteImage(lexiconEntity.Image.Url, cancellationToken);

            _context.Images.Remove(lexiconEntity.Image);
            _context.LexiconEntries.Remove(lexiconEntity);
            await _context.SaveChangesAsync(cancellationToken);

            return true;
        }

        public async Task<LexiconModel> GetItem(object id)
        {
            var lexiconEntity = await _context.LexiconEntries.FindAsync(id);
            await _context.Entry(lexiconEntity).Reference(p => p.Image).LoadAsync();

            return LexiconModel.FromEntity(lexiconEntity);
        }

        public async Task<IList<LexiconModel>> GetItems()
        {
            var lexiconEntities = await _context.LexiconEntries
                                                .Include(x => x.Image)
                                                .ToListAsync();

            return lexiconEntities.Select(LexiconModel.FromEntity).ToList();
        }

        public async Task<LexiconModel> UpdateItem(LexiconModel item, CancellationToken cancellationToken = default)
        {
            var lexiconEntity = await _context.LexiconEntries
                                                .Include(x => x.Image)
                                                .FirstOrDefaultAsync(u => u.Id == item.Id);

            _context.Entry(lexiconEntity).CurrentValues.SetValues(item.ToEntity());

            await _context.SaveChangesAsync(cancellationToken);

            return LexiconModel.FromEntity(lexiconEntity);
        }
    }
}
