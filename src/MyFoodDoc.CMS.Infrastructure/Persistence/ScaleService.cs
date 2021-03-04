using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using MyFoodDoc.Application.Abstractions;
using MyFoodDoc.Application.Entities.Psychogramm;
using MyFoodDoc.CMS.Application.Models;
using MyFoodDoc.CMS.Application.Persistence;
using MyFoodDoc.CMS.Application.Persistence.Base;
using MyFoodDoc.CMS.Infrastructure.AzureBlob;

namespace MyFoodDoc.CMS.Infrastructure.Persistence
{
    public class ScaleService : IScaleService
    {
        private readonly IApplicationContext _context;
        private readonly IImageBlobService _imageService;
        private readonly IQuestionService _questionService;

        public ScaleService(IApplicationContext context, IImageBlobService imageService, IQuestionService questionService)
        {
            this._context = context;
            this._imageService = imageService;
            this._questionService = questionService;
        }

        public async Task<ScaleModel> AddItem(ScaleModel item, CancellationToken cancellationToken = default)
        {
            var entity = item.ToEntity();
            await _context.Scales.AddAsync(entity, cancellationToken);

            await _context.SaveChangesAsync(cancellationToken);

            entity = await _context.Scales
                .Include(x => x.Image)
                .FirstOrDefaultAsync(u => u.Id == entity.Id, cancellationToken);

            return ScaleModel.FromEntity(entity);
        }

        public async Task<bool> DeleteItem(int id, CancellationToken cancellationToken = default)
        {
            var entity = await _context.Scales
                .Include(x => x.Image)
                .Include(x => x.Questions)
                .FirstOrDefaultAsync(u => u.Id == id, cancellationToken);

            foreach (var question in entity.Questions.ToList())
                await _questionService.DeleteItem(question.Id, cancellationToken);

            _context.Scales.Remove(entity);

            _context.Images.Remove(entity.Image);

            await _imageService.DeleteImage(entity.Image.Url, cancellationToken);

            await _context.SaveChangesAsync(cancellationToken);

            return true;
        }

        public async Task<ScaleModel> GetItem(int id, CancellationToken cancellationToken = default)
        {
            var entity = await _context.Scales
                .Include(x => x.Image)
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.Id == id, cancellationToken);

            return ScaleModel.FromEntity(entity);
        }

        public IQueryable<Scale> GetBaseQuery(string search)
        {
            IQueryable<Scale> baseQuery = _context.Scales.Include(x => x.Image);
            if (!string.IsNullOrWhiteSpace(search))
            {
                var searchString = $"%{search}%";
                baseQuery = baseQuery.Where(f => EF.Functions.Like(f.Title, searchString) || EF.Functions.Like(f.Text, searchString));
            }
            return baseQuery;
        }

        public async Task<PaginatedItems<ScaleModel>> GetItems(int take, int skip, string search, CancellationToken cancellationToken = default)
        {
            var entities = await GetBaseQuery(search).AsNoTracking().ToListAsync(cancellationToken);

            return new PaginatedItems<ScaleModel>()
            {
                Items = entities.Skip(skip).Take(take).Select(ScaleModel.FromEntity).OrderBy(x => x.Order).ToList(),
                TotalCount = entities.Count
            };
        }

        public async Task<ScaleModel> UpdateItem(ScaleModel item, CancellationToken cancellationToken = default)
        {
            var entity = await _context.Scales.FindAsync(new object[] { item.Id }, cancellationToken);

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
