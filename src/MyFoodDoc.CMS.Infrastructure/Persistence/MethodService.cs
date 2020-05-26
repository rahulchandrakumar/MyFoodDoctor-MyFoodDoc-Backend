using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using MyFoodDoc.Application.Abstractions;
using MyFoodDoc.Application.Entites.Abstractions;
using MyFoodDoc.Application.Enums;
using MyFoodDoc.CMS.Application.Models;
using MyFoodDoc.CMS.Application.Persistence;

namespace MyFoodDoc.CMS.Infrastructure.Persistence
{
    public class MethodService : IMethodService
    {
        private readonly IApplicationContext _context;
        private readonly IMethodMultipleChoiceService _methodMultipleChoiceService;

        public MethodService(IApplicationContext context, IMethodMultipleChoiceService methodMultipleChoiceService)
        {
            this._context = context;
            this._methodMultipleChoiceService = methodMultipleChoiceService;
        }
        public async Task<MethodModel> AddItem(MethodModel item, CancellationToken cancellationToken = default)
        {
            var entity = item.ToEntity();
            await _context.Methods.AddAsync(entity, cancellationToken);

            await _context.SaveChangesAsync(cancellationToken);

            entity = await _context.Methods
                .FirstOrDefaultAsync(u => u.Id == entity.Id, cancellationToken);

            return MethodModel.FromEntity(entity);
        }

        public async Task<bool> DeleteItem(int id, CancellationToken cancellationToken = default)
        {
            var entity = await _context.Methods
                .FirstOrDefaultAsync(u => u.Id == id, cancellationToken);

            if (entity.Type == MethodType.MultipleChoice)
                foreach (var methodMultipleChoice in await _context.MethodMultipleChoice.Where(x => x.MethodId == id).ToListAsync(cancellationToken))
                    await _methodMultipleChoiceService.DeleteItem(methodMultipleChoice.Id, cancellationToken);

            _context.Methods.Remove(entity);

            await _context.SaveChangesAsync(cancellationToken);

            return true;
        }

        public async Task<MethodModel> GetItem(int id, CancellationToken cancellationToken = default)
        {
            var entity = await _context.Methods
                .FirstOrDefaultAsync(x => x.Id == id, cancellationToken);

            return MethodModel.FromEntity(entity);
        }

        public IQueryable<Method> GetBaseQuery(int parentId, string search)
        {
            IQueryable<Method> baseQuery = _context.Methods.Where(x => x.TargetId == parentId);
            if (!string.IsNullOrWhiteSpace(search))
            {
                var searchString = $"%{search}%";
                baseQuery = baseQuery.Where(f => EF.Functions.Like(f.Title, searchString) || EF.Functions.Like(f.Text, searchString));
            }
            return baseQuery;
        }

        public async Task<IList<MethodModel>> GetItems(int parentId, int take, int skip, string search, CancellationToken cancellationToken = default)
        {
            var entities = await GetBaseQuery(parentId, search)
                .Skip(skip).Take(take)
                .ToListAsync(cancellationToken);

            return entities.Select(MethodModel.FromEntity).ToList();
        }

        public async Task<long> GetItemsCount(int parentId, string search, CancellationToken cancellationToken = default)
        {
            return await GetBaseQuery(parentId, search).CountAsync(cancellationToken);
        }

        public async Task<MethodModel> UpdateItem(MethodModel item, CancellationToken cancellationToken = default)
        {
            var entity = await _context.Methods.FindAsync(new object[] { item.Id }, cancellationToken);
            var newEntity = item.ToEntity();

            if (newEntity.Type != MethodType.MultipleChoice && entity.Type == MethodType.MultipleChoice)
                foreach (var methodMultipleChoice in await _context.MethodMultipleChoice.Where(x => x.MethodId == entity.Id).ToListAsync(cancellationToken))
                    await _methodMultipleChoiceService.DeleteItem(methodMultipleChoice.Id, cancellationToken);

            _context.Entry(entity).CurrentValues.SetValues(newEntity);

            await _context.SaveChangesAsync(cancellationToken);

            return await GetItem(entity.Id, cancellationToken);
        }
    }
}
