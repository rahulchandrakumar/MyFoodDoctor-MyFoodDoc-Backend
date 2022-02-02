using Microsoft.EntityFrameworkCore;
using MyFoodDoc.Application.Abstractions;
using MyFoodDoc.Application.Entities;
using MyFoodDoc.Functions.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace MyFoodDoc.Functions.Services
{
    public class UserStatsService : IUserStatsService
    {
        private readonly IApplicationContext _context;

        public const string APPLE_MONTHLY_SUBSCRIPTION = "de.myfooddoctor.app.monthly";
        public const string APPLE_SEMIANNUALLY_SUBSCRIPTION = "de.myfooddoctor.app.semiannually";
        public const string APPLE_ANNUALLY_SUBSCRIPTION = "de.myfooddoctor.app.annually";

        public const string GOOGLE_MONTHLY_SUBSCRIPTION = "medicum_monthly";
        public const string GOOGLE_SEMIANNUALLY_SUBSCRIPTION = "medicum_half_yearly";
        public const string GOOGLE_ANNUALLY_SUBSCRIPTION = "medicum_yearly";

        public UserStatsService(IApplicationContext context)
        {
            _context = context;
        }

        internal async Task<SubscriptionStatsDto> GetGoogleSubscriptionStatsAsync(CancellationToken cancellationToken)
        {
            var list = await _context.GooglePlayStoreSubscriptions
                            .Where(x => x.IsValid)
                            .GroupBy(x => x.SubscriptionId)
                            .Select(g => new Tuple<int, string>(g.Count(), g.Key)).ToListAsync(cancellationToken);

            var dto = new SubscriptionStatsDto()
            {
                HalfYearlySubscriptions = 0,
                MonthlySubscriptions = 0,
                YearlySubscriptions = 0,
                TotalSubscriptions = 0
            };
            foreach (var r in list)
            {
                if (r.Item2 == GOOGLE_MONTHLY_SUBSCRIPTION)
                {
                    dto.MonthlySubscriptions = r.Item1;
                    dto.TotalSubscriptions += r.Item1;
                }
                else if (r.Item2 == GOOGLE_SEMIANNUALLY_SUBSCRIPTION)
                {
                    dto.HalfYearlySubscriptions = r.Item1;
                    dto.TotalSubscriptions += r.Item1;
                }
                else if (r.Item2 == GOOGLE_ANNUALLY_SUBSCRIPTION)
                {
                    dto.YearlySubscriptions = r.Item1;
                    dto.TotalSubscriptions += r.Item1;
                }
            }

            return dto;
        }

        internal async Task<SubscriptionStatsDto> GetAppleSubscriptionStatsAsync(CancellationToken cancellationToken)
        {
            var list = await _context.AppStoreSubscriptions
                            .Where(x => x.IsValid)
                            .GroupBy(x => x.ProductId)
                            .Select(g => new Tuple<int, string>(g.Count(), g.Key)).ToListAsync(cancellationToken);

            var dto = new SubscriptionStatsDto()
            {
                HalfYearlySubscriptions = 0,
                MonthlySubscriptions = 0,
                YearlySubscriptions = 0,
                TotalSubscriptions = 0
            };
            foreach (var r in list)
            {
                if (r.Item2 == APPLE_MONTHLY_SUBSCRIPTION)
                {
                    dto.MonthlySubscriptions = r.Item1;
                    dto.TotalSubscriptions += r.Item1;
                }
                else if (r.Item2 == APPLE_SEMIANNUALLY_SUBSCRIPTION)
                {
                    dto.HalfYearlySubscriptions = r.Item1;
                    dto.TotalSubscriptions += r.Item1;
                }
                else if (r.Item2 == APPLE_ANNUALLY_SUBSCRIPTION)
                {
                    dto.YearlySubscriptions = r.Item1;
                    dto.TotalSubscriptions += r.Item1;
                }
            }

            return dto;
        }

        internal async Task<int> GetActiveUsersAsync(CancellationToken cancellationToken)
        {
            var counter = await _context.Meals.Where(x => x.Created > DateTime.Now.AddDays(-7)).Select(c => c.UserId).Distinct().CountAsync(cancellationToken);

            return counter;
        }

        public async Task<UserStatsDto> GetUserStatsAsync(CancellationToken cancellationToken)
        {
            var dto = new UserStatsDto();

            dto.AppleStats = await GetAppleSubscriptionStatsAsync(cancellationToken);
            dto.GoogleStats = await GetGoogleSubscriptionStatsAsync(cancellationToken);

            dto.ActiveUsers = await GetActiveUsersAsync(cancellationToken);

            return dto;
        }
    }
}
