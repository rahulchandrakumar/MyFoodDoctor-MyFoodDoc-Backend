using Microsoft.EntityFrameworkCore;
using MyFoodDoc.Application.Abstractions;
using MyFoodDoc.CMS.Application.Models;
using MyFoodDoc.CMS.Application.Persistence;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace MyFoodDoc.CMS.Infrastructure.Persistence
{
    public class LexiconService : ILexiconService
    {
        private readonly IApplicationContext _context;
        private readonly IImageService _imageService;

        public LexiconService(IApplicationContext context, IImageService imageService)
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
                                            .Include(i => i.Image)
                                            .FirstOrDefaultAsync(u => u.Id == lexiconEntity.Id);

            return LexiconModel.FromEntity(lexiconEntity);
        }

        public async Task<bool> DeleteItem(int id, CancellationToken cancellationToken = default)
        {
            var lexiconEntity = await _context.LexiconEntries
                                                .Include(i => i.Image)
                                                .FirstOrDefaultAsync(u => u.Id == id);
                    
            _context.Images.Remove(lexiconEntity.Image);
            _context.LexiconEntries.Remove(lexiconEntity);
            await _context.SaveChangesAsync(cancellationToken);

            return true;
        }

        public async Task<LexiconModel> GetItem(int id)
        {
            var lexiconEntity = await _context.LexiconEntries
                                                .Include(i => i.Image)
                                                .FirstOrDefaultAsync(u => u.Id == id);

            return LexiconModel.FromEntity(lexiconEntity);
        }

        public async Task<IList<LexiconModel>> GetItems()
        {
            var lexiconEntities = await _context.LexiconEntries
                                                .Include(i => i.Image)
                                                .ToListAsync();

            return lexiconEntities.Select(LexiconModel.FromEntity).ToList();
        }

        public async Task<LexiconModel> UpdateItem(LexiconModel item, CancellationToken cancellationToken = default)
        {
            var lexiconEntity = await _context.LexiconEntries
                                                .Include(i => i.Image)
                                                .FirstOrDefaultAsync(u => u.Id == item.Id);

            _context.Entry(lexiconEntity).CurrentValues.SetValues(item.ToEntity());

            await _context.SaveChangesAsync(cancellationToken);

            return LexiconModel.FromEntity(lexiconEntity);
        }
    }
}
