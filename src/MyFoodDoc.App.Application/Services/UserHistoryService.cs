using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using MyFoodDoc.App.Application.Abstractions;
using MyFoodDoc.App.Application.Models;
using MyFoodDoc.App.Application.Payloads.Diary;
using MyFoodDoc.Application.Abstractions;
using MyFoodDoc.Application.Entites.TrackedValus;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace MyFoodDoc.App.Application.Services
{
    public class UserHistoryService : IUserHistoryService
    {
        private readonly IApplicationContext _context;
        private readonly IMapper _mapper;

        public UserHistoryService(IApplicationContext context, IMapper mapper)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        public async Task<UserHistoryDto> GetAggregationAsync(string userId, CancellationToken cancellationToken = default)
        {
            var result = await _context.Users
                .Where(x => x.Id == userId)
                .ProjectTo<UserHistoryDto>(_mapper.ConfigurationProvider)
                .SingleOrDefaultAsync(cancellationToken);

            return result;
        }

        public async Task UpsertWeightHistoryAsync(string userId, WeightHistoryPayload payload, CancellationToken cancellationToken)
        {
            var entry = await _context.UserWeights.SingleOrDefaultAsync(x => x.UserId == userId && x.Date == payload.Date, cancellationToken);

            if (entry == null)
            {
                entry = new UserWeight
                {
                    UserId = userId,
                    Date = payload.Date
                };

                _context.UserWeights.Add(entry);
            }

            entry.Value = payload.Value;

            await _context.SaveChangesAsync(cancellationToken);
        }

        public async Task UpsertAbdonimalGirthHistoryAsync(string userId, AbdominalGirthHistoryPayload payload, CancellationToken cancellationToken = default)
        {
            var entry = await _context.UserAbdominalGirths.SingleOrDefaultAsync(x => x.UserId == userId && x.Date == payload.Date, cancellationToken);

            if (entry == null)
            {
                entry = new UserAbdominalGirth
                {
                    UserId = userId,
                    Date = payload.Date
                };

                _context.UserAbdominalGirths.Add(entry);
            }

            entry.Value = payload.Value;

            await _context.SaveChangesAsync(cancellationToken);
        }

        public async Task<IEnumerable<UserHistoryDtoWeight>> GetWeightHistoryAsync(string userId, CancellationToken cancellationToken)
        {
            var result = await _context.UserWeights
                .Where(x => x.UserId == userId )
                .ProjectTo<UserHistoryDtoWeight>(_mapper.ConfigurationProvider)
                .ToListAsync(cancellationToken);

            return result;
        }

        public async Task<IEnumerable<UserHistoryDtoAbdominalGirth>> GetAbdonimalGirthHistoryAsync(string userId, CancellationToken cancellationToken)
        {
            var result = await _context.UserAbdominalGirths
                .Where(x => x.UserId == userId)
                .ProjectTo<UserHistoryDtoAbdominalGirth>(_mapper.ConfigurationProvider)
                .ToListAsync(cancellationToken);

            return result;
        }
    }
}
