using Microsoft.EntityFrameworkCore;
using MyFoodDoc.Application.Abstractions;
using MyFoodDoc.CMS.Application.Models;
using MyFoodDoc.CMS.Application.Persistence;
using MyFoodDoc.CMS.Infrastructure.Mock;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace MyFoodDoc.CMS.Infrastructure.Persistence
{
    public class PatientService : IPatientService
    {
        private readonly IApplicationContext _context;

        public PatientService(IApplicationContext context)
        {
            this._context = context;
        }

        public async Task<PatientModel> GetItem(object id, CancellationToken cancellationToken = default)
        {
            var item = await _context.Users.FindAsync(id, cancellationToken);
            await _context.Entry(item).Reference(x => x.AbdonimalGirthHistory).LoadAsync(cancellationToken);
            await _context.Entry(item).Reference(x => x.BloodSugarLevelHistory).LoadAsync(cancellationToken);
            await _context.Entry(item).Reference(x => x.Motivations.Select(q => q.Motivation)).LoadAsync(cancellationToken);
            await _context.Entry(item).Reference(x => x.WeightHistory).LoadAsync(cancellationToken);

            return PatientModel.FromEntity(item);
        }

        public async Task<IList<PatientModel>> GetItems(CancellationToken cancellationToken = default)
        {
            var items = await _context.Users
                                        .Include(x => x.AbdonimalGirthHistory)
                                        .Include(x => x.BloodSugarLevelHistory)
                                        .Include(x => x.Motivations)
                                            .ThenInclude(x => x.Motivation)
                                        .Include(x => x.WeightHistory)
                                        .ToListAsync(cancellationToken);

            return items.Select(PatientModel.FromEntity).ToList();
        }
    }
}
