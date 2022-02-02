using Microsoft.EntityFrameworkCore;
using MyFoodDoc.Application.Abstractions;
using MyFoodDoc.Application.Entities.Methods;
using MyFoodDoc.CMS.Application.Models;
using MyFoodDoc.CMS.Application.Persistence;
using MyFoodDoc.CMS.Application.Persistence.Base;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace MyFoodDoc.CMS.Infrastructure.Persistence
{
    public class MethodTextService : IMethodTextService
    {
        private readonly IApplicationContext _context;

        public MethodTextService(IApplicationContext context)
        {
            this._context = context;
        }

        public async Task<MethodTextModel> AddItem(MethodTextModel item, CancellationToken cancellationToken = default)
        {
            var entity = item.ToEntity();
            await _context.MethodTexts.AddAsync(entity, cancellationToken);

            await _context.SaveChangesAsync(cancellationToken);

            entity = await _context.MethodTexts
                .FirstOrDefaultAsync(u => u.Id == entity.Id, cancellationToken);

            return MethodTextModel.FromEntity(entity);
        }

        public async Task<bool> DeleteItem(int id, CancellationToken cancellationToken = default)
        {
            var entity = await _context.MethodTexts.FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
            if (entity == null)
                return false;

            _context.MethodTexts.Remove(entity);
            await _context.SaveChangesAsync(cancellationToken);

            return true;
        }

        public async Task<MethodTextModel> GetItem(int id, CancellationToken cancellationToken = default)
        {
            var entity = await _context.MethodTexts
                .FirstOrDefaultAsync(x => x.Id == id, cancellationToken);

            return MethodTextModel.FromEntity(entity);
        }

        public IQueryable<MethodText> GetBaseQuery(int parentId, string search)
        {
            IQueryable<MethodText> baseQuery = _context.MethodTexts.Where(x => x.MethodId == parentId);
            if (!string.IsNullOrWhiteSpace(search))
            {
                var searchString = $"%{search}%";
                baseQuery = baseQuery.Where(f => EF.Functions.Like(f.Title, searchString));
            }
            return baseQuery;
        }

        public async Task<PaginatedItems<MethodTextModel>> GetItems(int parentId, int take, int skip, string search, CancellationToken cancellationToken = default)
        {
            var entities = await GetBaseQuery(parentId, search).AsNoTracking().ToListAsync(cancellationToken);

            return new PaginatedItems<MethodTextModel>()
            {
                Items = entities.Skip(skip).Take(take).Select(MethodTextModel.FromEntity).ToList(),
                TotalCount = entities.Count
            };
        }

        public async Task<MethodTextModel> UpdateItem(MethodTextModel item, CancellationToken cancellationToken = default)
        {
            var entity = await _context.MethodTexts.FindAsync(new object[] { item.Id }, cancellationToken);

            _context.Entry(entity).CurrentValues.SetValues(item.ToEntity());

            await _context.SaveChangesAsync(cancellationToken);

            return await GetItem(entity.Id, cancellationToken);
        }
    }
}
