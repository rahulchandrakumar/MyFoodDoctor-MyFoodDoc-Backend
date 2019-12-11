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
    public class InsuranceService : IInsuranceService
    {
        private readonly IApplicationContext _context;

        public InsuranceService(IApplicationContext context)
        {
            this._context = context;
        }

        public async Task<InsuranceModel> GetItem(int id, CancellationToken cancellationToken = default)
        {
            return InsuranceModel.FromEntity(await _context.Insurances.FindAsync(new object[] { id }, cancellationToken));
        }

        public async Task<IList<InsuranceModel>> GetItems(CancellationToken cancellationToken = default)
        {
            return (await _context.Insurances.ToListAsync(cancellationToken)).Select(InsuranceModel.FromEntity).ToList();
        }
    }
}
