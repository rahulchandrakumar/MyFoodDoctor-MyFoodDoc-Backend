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
    public class MotivationService : IMotivationService
    {
        private readonly IApplicationContext _context;

        public MotivationService(IApplicationContext context)
        {
            this._context = context;
        }

        public async Task<IList<MotivationModel>> GetItems(CancellationToken cancellationToken = default)
        {
            return (await _context.Motivations.ToListAsync(cancellationToken)).Select(MotivationModel.FromEntity).ToList();
        }
    }
}
