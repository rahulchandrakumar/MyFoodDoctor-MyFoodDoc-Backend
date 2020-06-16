using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using MyFoodDoc.Application.Abstractions;
using MyFoodDoc.CMS.Application.Models;
using MyFoodDoc.CMS.Application.Persistence;

namespace MyFoodDoc.CMS.Infrastructure.Persistence
{
    public class DietService : IDietService
    {
        private readonly IApplicationContext _context;

        public DietService(IApplicationContext context)
        {
            this._context = context;
        }

        public async Task<DietModel> GetItem(int id, CancellationToken cancellationToken = default)
        {
            return DietModel.FromEntity(await _context.Diets.FindAsync(new object[] { id }, cancellationToken));
        }

        public async Task<IList<DietModel>> GetItems(CancellationToken cancellationToken = default)
        {
            return (await _context.Diets.ToListAsync(cancellationToken)).Select(DietModel.FromEntity).ToList();
        }
    }
}
