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
    public class IndicationService : IIndicationService
    {
        private readonly IApplicationContext _context;

        public IndicationService(IApplicationContext context)
        {
            this._context = context;
        }

        public async Task<IList<IndicationModel>> GetItems(CancellationToken cancellationToken = default)
        {
            return (await _context.Indications.ToListAsync(cancellationToken)).Select(IndicationModel.FromEntity).ToList();
        }
    }
}
