using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using MyFoodDoc.App.Application.Abstractions;
using MyFoodDoc.App.Application.Models;
using MyFoodDoc.App.Application.Payloads.Diary;
using MyFoodDoc.Application.Abstractions;
using MyFoodDoc.Application.Entities.TrackedValues;
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
            var weightHistory = await GetWeightHistoryAsync(userId, cancellationToken);

            var abdominalGirthHistory = await GetAbdominalGirthHistoryAsync(userId, cancellationToken);

            return new UserHistoryDto
            {
                Weight = weightHistory,
                AbdominalGirth = abdominalGirthHistory
            };
        }

        public async Task UpsertWeightHistoryAsync(string userId, WeightHistoryPayload payload, CancellationToken cancellationToken)
        {
            var entry = await _context.UserWeights.SingleOrDefaultAsync(x => x.UserId == userId && x.Date == payload.Date, cancellationToken);

            if (entry == null)
            {
                entry = new UserWeight
                {
                    UserId = userId,
                    Date = payload.Date,
                    Value = payload.Value
                };

                await _context.UserWeights.AddAsync(entry, cancellationToken);
            }
            else
            {
                entry.Value = payload.Value;

                _context.UserWeights.Update(entry);
            }

            await _context.SaveChangesAsync(cancellationToken);
        }

        public async Task UpsertAbdominalGirthHistoryAsync(string userId, AbdominalGirthHistoryPayload payload, CancellationToken cancellationToken = default)
        {
            var entry = await _context.UserAbdominalGirths.SingleOrDefaultAsync(x => x.UserId == userId && x.Date == payload.Date, cancellationToken);

            if (entry == null)
            {
                entry = new UserAbdominalGirth
                {
                    UserId = userId,
                    Date = payload.Date,
                    Value = payload.Value
                };

                await _context.UserAbdominalGirths.AddAsync(entry, cancellationToken);
            }
            else
            {
                entry.Value = payload.Value;

                _context.UserAbdominalGirths.Update(entry);
            }

            await _context.SaveChangesAsync(cancellationToken);
        }

        public async Task<UserHistoryDtoWeight> GetWeightHistoryAsync(string userId, CancellationToken cancellationToken)
        {
            var weights = await _context.UserWeights
                .Where(x => x.UserId == userId)
                .ToListAsync(cancellationToken);

            return _mapper.Map<UserHistoryDtoWeight>(weights);
        }

        public async Task<UserHistoryDtoAbdominalGirth> GetAbdominalGirthHistoryAsync(string userId, CancellationToken cancellationToken)
        {
            var abdominalGirths = await _context.UserAbdominalGirths
                .Where(x => x.UserId == userId)
                .ToListAsync(cancellationToken);

            return _mapper.Map<UserHistoryDtoAbdominalGirth>(abdominalGirths);
        }
    }
}
