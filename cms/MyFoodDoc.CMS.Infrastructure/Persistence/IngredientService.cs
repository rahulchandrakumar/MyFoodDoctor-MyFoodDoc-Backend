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

        public async Task<IList<IngredientModel>> GetItems(CancellationToken cancellationToken = default)
        {
            return (await _context.Ingredients.ToListAsync(cancellationToken)).Select(IngredientModel.FromEntity).ToList();
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
