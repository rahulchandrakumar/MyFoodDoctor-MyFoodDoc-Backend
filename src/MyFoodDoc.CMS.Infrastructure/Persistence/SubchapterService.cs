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

namespace MyFoodDoc.CMS.Infrastructure.Persistence
{
    public class SubchapterService : ISubchapterService
    {
        private readonly IApplicationContext _context;

        public SubchapterService(IApplicationContext context)
        {
            this._context = context;
        }

        public async Task<SubchapterModel> AddItem(SubchapterModel item, CancellationToken cancellationToken = default)
        {
            var entity = item.ToEntity();
            await _context.Subchapters.AddAsync(entity, cancellationToken);

            await _context.SaveChangesAsync(cancellationToken);

            entity = await _context.Subchapters
                .FirstOrDefaultAsync(u => u.Id == entity.Id, cancellationToken);

            return SubchapterModel.FromEntity(entity);
        }

        public async Task<bool> DeleteItem(int id, CancellationToken cancellationToken = default)
        {
            var entity = await _context.Subchapters.FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
            if (entity == null)
                return false;

            _context.Subchapters.Remove(entity);
            await _context.SaveChangesAsync(cancellationToken);

            return true;
        }

        public async Task<SubchapterModel> GetItem(int id, CancellationToken cancellationToken = default)
        {
            var entity = await _context.Subchapters
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.Id == id, cancellationToken);

            return SubchapterModel.FromEntity(entity);
        }

        public IQueryable<Subchapter> GetBaseQuery(int parentId, string search)
        {
            IQueryable<Subchapter> baseQuery = _context.Subchapters.Where(x => x.ChapterId == parentId);

            if (!string.IsNullOrWhiteSpace(search))
            {
                var searchString = $"%{search}%";
                baseQuery = baseQuery.Where(f => EF.Functions.Like(f.Title, searchString) || EF.Functions.Like(f.Text, searchString));
            }
            return baseQuery;
        }

        public async Task<PaginatedItems<SubchapterModel>> GetItems(int parentId, int take, int skip, string search, CancellationToken cancellationToken = default)
        {
            var entities = await GetBaseQuery(parentId, search).AsNoTracking().ToListAsync(cancellationToken);

            return new PaginatedItems<SubchapterModel>()
            {
                Items = entities.OrderBy(x => x.Order).Skip(skip).Take(take).Select(SubchapterModel.FromEntity).ToList(),
                TotalCount = entities.Count
            };
        }

        public async Task<SubchapterModel> UpdateItem(SubchapterModel item, CancellationToken cancellationToken = default)
        {
            var entity = await _context.Subchapters.FindAsync(new object[] { item.Id }, cancellationToken);

            _context.Entry(entity).CurrentValues.SetValues(item.ToEntity());

            await _context.SaveChangesAsync(cancellationToken);

            return await GetItem(entity.Id, cancellationToken);
        }
    }
}
