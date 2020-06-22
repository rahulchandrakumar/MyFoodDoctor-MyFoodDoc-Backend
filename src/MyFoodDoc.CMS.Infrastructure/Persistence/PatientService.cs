using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using MyFoodDoc.Application.Abstractions;
using MyFoodDoc.Application.Entities;
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
        private const int _maxHistoryAmount = 10;
        private readonly IApplicationContext _context;

        private readonly IMemoryCache _memoryCache;
        private const string cachePrefix = nameof(PatientService) + "_";
        private readonly TimeSpan cacheLife = TimeSpan.FromMinutes(15);

        public PatientService(IApplicationContext context, IMemoryCache memoryCache)
        {
            this._context = context;
            this._memoryCache = memoryCache;
        }

        public async Task<PatientModel> GetItem(string id, CancellationToken cancellationToken = default)
        {
            var item = await _context.Users
                                         .Include(x => x.AbdominalGirthHistory)
                                         .Include(x => x.Motivations)
                                            .ThenInclude(x => x.Motivation)
                                         .Include(x => x.Indications)
                                            .ThenInclude(x => x.Indication)
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
                                        .Include(x => x.Indications)
                                            .ThenInclude(x => x.Indication)
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

        public async Task<IList<HistoryModel<int>>> FullUserHistory(CancellationToken cancellationToken = default)
        {
            var cacheKey = cachePrefix + nameof(FullUserHistory);

            var cached = _memoryCache.Get(cacheKey) as List<HistoryModel<int>>;
            if (cached != null)
                return cached;

            var totalCount = await _context.Users.CountAsync(cancellationToken);
            if (totalCount == 0)
                return new List<HistoryModel<int>>() { new HistoryModel<int>() { Created = DateTimeOffset.Now, Value = 0 } };

            var firstDate = await _context.Users.Select(x => x.Created).MinAsync(cancellationToken);
            if (firstDate == default)
                firstDate = new DateTime(2019, 1, 1);
            var totalDays = (DateTime.Now - firstDate).Days;
            var dayresult = await _context.Users
                                .GroupBy(x => x.Created.Date)
                                .OrderBy(x => x.Key)
                                .Select(x => new { Date = x.Key, Count = x.Count() })
                                .ToListAsync(cancellationToken);
            if (totalDays < _maxHistoryAmount)
            {                
                return dayresult.Select(x => new HistoryModel<int>() { 
                    Created = x.Date, 
                    Value = dayresult.Where(y => y.Date <= x.Date).Sum(y => y.Count) 
                }).ToList();
            }

            var dates = new List<DateTime>() { firstDate };
            var step = (double)totalDays / _maxHistoryAmount;
            for (int i = 1; i < _maxHistoryAmount - 1; i++)
            {
                dates.Add(firstDate.AddDays(i * step));
            }
            dates.Add(DateTime.Now);

            var totalResult = dates.Select(r => new HistoryModel<int>()
            {
                Created = r,
                Value = dayresult.Where(x => x.Date <= r).Sum(x => x.Count)
            });            

            var result = totalResult.OrderBy(x => x.Created).ToList();

            _memoryCache.Set(cacheKey, result, cacheLife);

            return result;
        }
    }
}
