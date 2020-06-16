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
    public class MotivationService : IMotivationService
    {
        private readonly IApplicationContext _context;

        public MotivationService(IApplicationContext context)
        {
            this._context = context;
        }

        public async Task<MotivationModel> GetItem(int id, CancellationToken cancellationToken = default)
        {
            return MotivationModel.FromEntity(await _context.Motivations.FindAsync(new object[] { id }, cancellationToken));
        }

        public async Task<IList<MotivationModel>> GetItems(CancellationToken cancellationToken = default)
        {
            return (await _context.Motivations.ToListAsync(cancellationToken)).Select(MotivationModel.FromEntity).ToList();
        }
    }
}
