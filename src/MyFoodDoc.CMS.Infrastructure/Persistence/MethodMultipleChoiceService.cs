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
    public class MethodMultipleChoiceService : IMethodMultipleChoiceService
    {
        private readonly IApplicationContext _context;

        public MethodMultipleChoiceService(IApplicationContext context)
        {
            this._context = context;
        }

        public async Task<MethodMultipleChoiceModel> AddItem(MethodMultipleChoiceModel item, CancellationToken cancellationToken = default)
        {
            var entity = item.ToEntity();
            await _context.MethodMultipleChoice.AddAsync(entity, cancellationToken);

            await _context.SaveChangesAsync(cancellationToken);

            entity = await _context.MethodMultipleChoice
                .FirstOrDefaultAsync(u => u.Id == entity.Id, cancellationToken);

            return MethodMultipleChoiceModel.FromEntity(entity);
        }

        public async Task<bool> DeleteItem(int id, CancellationToken cancellationToken = default)
        {
            var entity = await _context.MethodMultipleChoice.FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
            if (entity == null)
                return false;

            _context.MethodMultipleChoice.Remove(entity);
            await _context.SaveChangesAsync(cancellationToken);

            return true;
        }

        public async Task<MethodMultipleChoiceModel> GetItem(int id, CancellationToken cancellationToken = default)
        {
            var entity = await _context.MethodMultipleChoice
                .FirstOrDefaultAsync(x => x.Id == id, cancellationToken);

            return MethodMultipleChoiceModel.FromEntity(entity);
        }

        public IQueryable<MethodMultipleChoice> GetBaseQuery(int parentId, string search)
        {
            IQueryable<MethodMultipleChoice> baseQuery = _context.MethodMultipleChoice.Where(x => x.MethodId == parentId);
            if (!string.IsNullOrWhiteSpace(search))
            {
                var searchString = $"%{search}%";
                baseQuery = baseQuery.Where(f => EF.Functions.Like(f.Title, searchString));
            }
            return baseQuery;
        }

        public async Task<PaginatedItems<MethodMultipleChoiceModel>> GetItems(int parentId, int take, int skip, string search, CancellationToken cancellationToken = default)
        {
            var entities = await GetBaseQuery(parentId, search).AsNoTracking().ToListAsync(cancellationToken);

            return new PaginatedItems<MethodMultipleChoiceModel>()
            {
                Items = entities.Skip(skip).Take(take).Select(MethodMultipleChoiceModel.FromEntity).ToList(),
                TotalCount = entities.Count
            };
        }

        public async Task<MethodMultipleChoiceModel> UpdateItem(MethodMultipleChoiceModel item, CancellationToken cancellationToken = default)
        {
            var entity = await _context.MethodMultipleChoice.FindAsync(new object[] { item.Id }, cancellationToken);

            _context.Entry(entity).CurrentValues.SetValues(item.ToEntity());

            await _context.SaveChangesAsync(cancellationToken);

            return await GetItem(entity.Id, cancellationToken);
        }
    }
}
