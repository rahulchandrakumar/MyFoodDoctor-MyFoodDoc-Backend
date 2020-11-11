using Microsoft.EntityFrameworkCore;
using MyFoodDoc.Application.Abstractions;
using MyFoodDoc.Application.Entities;
using MyFoodDoc.CMS.Application.FilterModels;
using MyFoodDoc.CMS.Application.Models;
using MyFoodDoc.CMS.Application.Persistence;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MyFoodDoc.CMS.Application.Persistence.Base;

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
                var searchString = $"%{search}%";
                baseQuery = baseQuery.Where(f => EF.Functions.Like(f.FoodName, searchString) || EF.Functions.Like(f.ServingDescription, searchString));
            }

            //TODO: Check relevance
            /*
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
            */

            return baseQuery;
        }

        public async Task<PaginatedItems<IngredientModel>> GetItems(int take, int skip, string search, IngredientFilter filter, CancellationToken cancellationToken = default)
        {
            var entities = await GetBaseQuery(search, filter).AsNoTracking().ToListAsync(cancellationToken);

            return new PaginatedItems<IngredientModel>()
            {
                Items = entities.Skip(skip).Take(take).Select(IngredientModel.FromEntity).ToList(),
                TotalCount = entities.Count
            };
        }

        public async Task<IngredientModel> UpdateItem(IngredientModel item, CancellationToken cancellationToken = default)
        {
            var entity = item.ToEntity();

            var orig = await _context.Ingredients.FindAsync(new object[] { entity.Id }, cancellationToken);

            entity.CaloriesExternal = orig.CaloriesExternal;
            entity.CarbohydrateExternal = orig.CarbohydrateExternal;
            entity.ProteinExternal = orig.ProteinExternal;
            entity.FatExternal = orig.FatExternal;
            entity.SaturatedFatExternal = orig.SaturatedFatExternal;
            entity.PolyunsaturatedFatExternal = orig.PolyunsaturatedFatExternal;
            entity.MonounsaturatedFatExternal = orig.MonounsaturatedFatExternal;
            entity.CholesterolExternal = orig.CholesterolExternal;
            entity.SodiumExternal = orig.SodiumExternal;
            entity.PotassiumExternal = orig.PotassiumExternal;
            entity.FiberExternal = orig.FiberExternal;
            entity.SugarExternal = orig.SugarExternal;

            _context.Entry(orig).CurrentValues.SetValues(entity);

            await _context.SaveChangesAsync(cancellationToken);

            return IngredientModel.FromEntity(orig);
        }
    }
}
