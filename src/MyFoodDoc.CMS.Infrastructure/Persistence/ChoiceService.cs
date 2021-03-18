using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using MyFoodDoc.Application.Abstractions;
using MyFoodDoc.Application.Entities.Psychogramm;
using MyFoodDoc.CMS.Application.Models;
using MyFoodDoc.CMS.Application.Persistence;
using MyFoodDoc.CMS.Application.Persistence.Base;

namespace MyFoodDoc.CMS.Infrastructure.Persistence
{
    public class ChoiceService : IChoiceService
    {
        private readonly IApplicationContext _context;

        public ChoiceService(IApplicationContext context)
        {
            this._context = context;
        }

        public async Task<ChoiceModel> AddItem(ChoiceModel item, CancellationToken cancellationToken = default)
        {
            var entity = item.ToEntity();
            await _context.Choices.AddAsync(entity, cancellationToken);

            await _context.SaveChangesAsync(cancellationToken);

            entity = await _context.Choices
                .FirstOrDefaultAsync(u => u.Id == entity.Id, cancellationToken);

            return ChoiceModel.FromEntity(entity);
        }

        public async Task<bool> DeleteItem(int id, CancellationToken cancellationToken = default)
        {
            var entity = await _context.Choices.FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
            if (entity == null)
                return false;

            _context.Choices.Remove(entity);
            await _context.SaveChangesAsync(cancellationToken);

            return true;
        }

        public async Task<ChoiceModel> GetItem(int id, CancellationToken cancellationToken = default)
        {
            var entity = await _context.Choices
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.Id == id, cancellationToken);

            return ChoiceModel.FromEntity(entity);
        }

        public IQueryable<Choice> GetBaseQuery(int parentId, string search)
        {
            IQueryable<Choice> baseQuery = _context.Choices.Where(x => x.QuestionId == parentId);

            if (!string.IsNullOrWhiteSpace(search))
            {
                var searchString = $"%{search}%";
                baseQuery = baseQuery.Where(f => EF.Functions.Like(f.Text, searchString));
            }
            return baseQuery;
        }

        public async Task<PaginatedItems<ChoiceModel>> GetItems(int parentId, int take, int skip, string search, CancellationToken cancellationToken = default)
        {
            var entities = await GetBaseQuery(parentId, search).AsNoTracking().ToListAsync(cancellationToken);

            return new PaginatedItems<ChoiceModel>()
            {
                Items = entities.OrderBy(x => x.Order).Skip(skip).Take(take).Select(ChoiceModel.FromEntity).ToList(),
                TotalCount = entities.Count
            };
        }

        public async Task<ChoiceModel> UpdateItem(ChoiceModel item, CancellationToken cancellationToken = default)
        {
            var entity = await _context.Choices.FindAsync(new object[] { item.Id }, cancellationToken);

            _context.Entry(entity).CurrentValues.SetValues(item.ToEntity());

            await _context.SaveChangesAsync(cancellationToken);

            return await GetItem(entity.Id, cancellationToken);
        }
    }
}
