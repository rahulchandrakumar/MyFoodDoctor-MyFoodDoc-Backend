﻿using Microsoft.EntityFrameworkCore;
using MyFoodDoc.Application.Abstractions;
using MyFoodDoc.Application.Entites;
using MyFoodDoc.CMS.Application.FilterModels;
using MyFoodDoc.CMS.Application.Models;
using MyFoodDoc.CMS.Application.Persistence;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace MyFoodDoc.CMS.Infrastructure.Persistence
{
    public class IngredientService : IIngredientService
    {
        private IApplicationContext _context;
        public IngredientService(IApplicationContext context)
        {
            this._context = context;
        }

        public async Task<IngredientModel> AddItem(IngredientModel item, CancellationToken cancellationToken = default)
        {
            var entity = item.ToEntity();

            await _context.Ingredients.AddAsync(entity, cancellationToken);

            await _context.SaveChangesAsync(cancellationToken);

            return IngredientModel.FromEntity(entity);
        }

        public async Task<bool> DeleteItem(int id, CancellationToken cancellationToken = default)
        {
            var item = await _context.Ingredients.FindAsync(new object[] { id }, cancellationToken);
            if (item == null)
                return false;

            _context.Ingredients.Remove(item);
            await _context.SaveChangesAsync(cancellationToken);

            return true;
        }

        public async Task<IngredientModel> GetItem(int id, CancellationToken cancellationToken = default)
        {
            return IngredientModel.FromEntity(await _context.Ingredients.FindAsync(new object[] { id }, cancellationToken));
        }

        private IQueryable<Ingredient> GetBaseQuery(string search, IngredientFilter filter)
        {
            IQueryable<Ingredient> baseQuery = _context.Ingredients;
            if (!string.IsNullOrWhiteSpace(search))
            {
                var searchstring = $"%{search}%";
                baseQuery = baseQuery.Where(f => EF.Functions.Like(f.ExternalKey, searchstring) || EF.Functions.Like(f.Name, searchstring));
            }
            if (filter != null)
            {
                switch (filter.State)
                {
                    case IngredientFilterState.HaveToSpecify:
                        baseQuery = baseQuery.Where(x => x.Amount == null && x.Meals.Count != 0);
                        break;
                    case IngredientFilterState.Specified:
                        baseQuery = baseQuery.Where(x => x.Amount != null);
                        break;
                    case IngredientFilterState.NotSpecified:
                        baseQuery = baseQuery.Where(x => x.Amount == null && x.Meals.Count == 0);
                        break;
                }
            }
            return baseQuery;
        }

        public async Task<IList<IngredientModel>> GetItems(int take, int skip, string search, IngredientFilter filter, CancellationToken cancellationToken = default)
        {
            return (await GetBaseQuery(search, filter).Skip(skip).Take(take).ToListAsync(cancellationToken)).Select(IngredientModel.FromEntity).ToList();
        }

        public async Task<long> GetItemsCount(string search, IngredientFilter filter, CancellationToken cancellationToken = default)
        {
            return await GetBaseQuery(search, filter).CountAsync(cancellationToken);
        }

        public async Task<IngredientModel> UpdateItem(IngredientModel item, CancellationToken cancellationToken = default)
        {
            var entity = item.ToEntity();

            var orig = await _context.Ingredients.FindAsync(new object[] { entity.Id }, cancellationToken);

            _context.Entry(orig).CurrentValues.SetValues(entity);

            await _context.SaveChangesAsync(cancellationToken);

            return IngredientModel.FromEntity(orig);
        }
    }
}