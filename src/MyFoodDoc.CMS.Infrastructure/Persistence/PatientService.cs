using Microsoft.EntityFrameworkCore;
using MyFoodDoc.Application.Abstractions;
using MyFoodDoc.Application.Entites;
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
                                         .Include(x => x.AbdominalGirthHistory)
                                         .Include(x => x.Motivations)
                                             .ThenInclude(x => x.Motivation)
                                         .Include(x => x.WeightHistory)
                                         .AsNoTracking()
                                         .FirstOrDefaultAsync(x => x.Id == id, cancellationToken);

            return PatientModel.FromEntity(item);
        }

        public async Task<IList<PatientModel>> GetItems(int take, int skip, string search, CancellationToken cancellationToken = default)
        {
            IQueryable<User> baseQuery = _context.Users
                                        .Include(x => x.AbdominalGirthHistory)
                                        .Include(x => x.Motivations)
                                            .ThenInclude(x => x.Motivation)
                                        .Include(x => x.WeightHistory)
                                        .AsNoTracking();
            if (!string.IsNullOrWhiteSpace(search))
            {
                var searchstring = $"%{search}%";
                baseQuery = baseQuery.Where(f => EF.Functions.Like(f.Email, searchstring) || EF.Functions.Like(f.Gender.ToString(), searchstring));
            }

            return (await baseQuery.Take(take).Skip(skip).ToListAsync(cancellationToken)).Select(PatientModel.FromEntity).ToList();
        }

        public async Task<long> GetItemsCount(string search, CancellationToken cancellationToken = default)
        {
            IQueryable<User> baseQuery = _context.Users
                                        .AsNoTracking();
            if (!string.IsNullOrWhiteSpace(search))
            {
                var searchstring = $"%{search}%";
                baseQuery = baseQuery.Where(f => EF.Functions.Like(f.Email, searchstring) || EF.Functions.Like(f.Gender.ToString(), searchstring));
            }
            return await baseQuery.CountAsync(cancellationToken);
        }
    }
}
