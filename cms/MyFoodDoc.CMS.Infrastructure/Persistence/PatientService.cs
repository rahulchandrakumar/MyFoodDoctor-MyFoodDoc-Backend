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
    public class PatientService : IPatientService
    {
        private readonly IApplicationContext _context;

        public PatientService(IApplicationContext context)
        {
            this._context = context;
        }

        public async Task<PatientModel> GetItem(string id, CancellationToken cancellationToken = default)
        {
            var item = await _context.Users
                                         .Include(x => x.AbdonimalGirthHistory)
                                         .Include(x => x.BloodSugarLevelHistory)
                                         .Include(x => x.Motivations)
                                             .ThenInclude(x => x.Motivation)
                                         .Include(x => x.WeightHistory)
                                         .FirstOrDefaultAsync(x => x.Id == id);

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
