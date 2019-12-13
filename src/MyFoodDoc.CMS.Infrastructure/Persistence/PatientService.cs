using Microsoft.EntityFrameworkCore;
using MyFoodDoc.Application.Abstractions;
using MyFoodDoc.Application.Entites;
using MyFoodDoc.Application.Enums;
using MyFoodDoc.CMS.Application.Models;
using MyFoodDoc.CMS.Application.Persistence;
using System;
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

        public IQueryable<User> GetBaseQuery(string search)
        {
            IQueryable<User> baseQuery = _context.Users;
            if (!string.IsNullOrWhiteSpace(search))
            {
                var searchstring = $"%{search}%";
                var genders = Enum.GetValues(typeof(Gender)).Cast<Gender>().Where(v => v.ToString().ToUpper() == search.ToUpper()).ToList();
                if (genders.Count > 0)
                {
                    var gender = genders.First();
                    baseQuery = baseQuery.Where(f => EF.Functions.Like(f.Email, searchstring) || f.Gender == gender);
                }
                else
                {
                    baseQuery = baseQuery.Where(f => EF.Functions.Like(f.Email, searchstring));
                }
            }
            return baseQuery;
        }

        public async Task<IList<PatientModel>> GetItems(int take, int skip, string search, CancellationToken cancellationToken = default)
        {
            var queryResult = await GetBaseQuery(search)
                                        .Include(x => x.AbdominalGirthHistory)
                                        .Include(x => x.Motivations)
                                            .ThenInclude(x => x.Motivation)
                                        .Include(x => x.WeightHistory)
                                        .Skip(skip).Take(take)
                                        .AsNoTracking()
                                        .ToListAsync(cancellationToken);
            

            return queryResult.Select(PatientModel.FromEntity).ToList();
        }

        public async Task<long> GetItemsCount(string search, CancellationToken cancellationToken = default)
        {
            return await GetBaseQuery(search).AsNoTracking().CountAsync(cancellationToken);
        }
    }
}
