using Microsoft.EntityFrameworkCore;
using MyFoodDoc.Application.Abstractions;
using MyFoodDoc.CMS.Application.Models;
using MyFoodDoc.CMS.Application.Persistence;
using MyFoodDoc.CMS.Infrastructure.Mock;
using System.Collections.Generic;
using System.Linq;
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

        public async Task<PatientModel> GetItem(object id)
        {
            var item = await _context.Users.FindAsync(id);
            await _context.Entry(item).Reference(p => p.AbdonimalGirthHistory).LoadAsync();
            await _context.Entry(item).Reference(p => p.BloodSugarLevelHistory).LoadAsync();
            await _context.Entry(item).Reference(p => p.Motivations).LoadAsync();
            await _context.Entry(item).Reference(p => p.WeightHistory).LoadAsync();

            return PatientModel.FromEntity(item);
        }

        public async Task<IList<PatientModel>> GetItems()
        {
            var items = await _context.Users
                                        .Include(x => x.AbdonimalGirthHistory)
                                        .Include(x => x.BloodSugarLevelHistory)
                                        .Include(x => x.Motivations)
                                        .Include(x => x.WeightHistory)
                                        .ToListAsync();

            return items.Select(PatientModel.FromEntity).ToList();
        }
    }
}
