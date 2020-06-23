using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using MyFoodDoc.App.Application.Abstractions;
using MyFoodDoc.App.Application.Configuration;
using MyFoodDoc.App.Application.Exceptions;
using MyFoodDoc.App.Application.Models;
using MyFoodDoc.App.Application.Payloads.Method;
using MyFoodDoc.Application.Abstractions;
using MyFoodDoc.Application.Entities;
using MyFoodDoc.Application.Entities.Abstractions;
using MyFoodDoc.Application.Entities.Methods;
using MyFoodDoc.Application.Enums;

namespace MyFoodDoc.App.Application.Services
{
    public class MethodService : IMethodService
    {
        private readonly IApplicationContext _context;
        private readonly ITargetService _targetService;
        private readonly int _statisticsPeriod;

        public MethodService(IApplicationContext context, ITargetService targetService, IOptions<StatisticsOptions> statisticsOptions)
        {
            _context = context;
            _targetService = targetService;
            _statisticsPeriod = statisticsOptions.Value.Period > 0 ? statisticsOptions.Value.Period : 7;
        }

        public async Task<ICollection<MethodDto>> GetAsync(string userId, CancellationToken cancellationToken)
        {
            var result = new List<MethodDto>();

            var triggeredTargetIds = (await _targetService.GetAsync(userId, cancellationToken)).SelectMany(x => x.Targets).Select(x=> x.Id).ToList();

            var userDiets = await _context.UserDiets.Where(x => x.UserId == userId).Select(x => x.DietId).ToListAsync(cancellationToken);
            var userIndications = await _context.UserIndications.Where(x => x.UserId == userId).Select(x => x.IndicationId).ToListAsync(cancellationToken);
            var userMotivations = await _context.UserMotivations.Where(x => x.UserId == userId).Select(x => x.MotivationId).ToListAsync(cancellationToken);

            var availableMethodIds = _context.DietMethods.Where(x => userDiets.Contains(x.DietId)).Select(x => x.MethodId)
                .Union(_context.IndicationMethods.Where(x => userIndications.Contains(x.IndicationId)).Select(x => x.MethodId))
                .Union(_context.MotivationMethods.Where(x => userMotivations.Contains(x.MotivationId)).Select(x => x.MethodId)).Distinct();


            var methods = await _context.Methods
                .Include(x => x.Targets)
                .Include(x=> x.Image)
                .Where(x => availableMethodIds.Contains(x.Id) && x.Targets.Any(y=> triggeredTargetIds.Contains(y.TargetId)))
                .ToListAsync(cancellationToken);

            if (!methods.Any())
                return result;

            var methodIds = methods.Select(x => x.Id);
            
            var userMethodShowHistory = (await _context.UserMethodShowHistory
                .Where(x => x.UserId == userId && x.Date > DateTime.Now.AddDays(-_statisticsPeriod) &&
                            methodIds.Contains(x.MethodId))
                .ToListAsync(cancellationToken))
                .GroupBy(g => g.MethodId)
                .Select(x => new { x.Key, Count = x.Count() });

            Method methodToShow = methods.FirstOrDefault(x => userMethodShowHistory.All(y => y.Key != x.Id));

            if (methodToShow == null)
            {
                var userMethodShowHistoryItem = userMethodShowHistory.OrderBy(x => x.Count).First();

                methodToShow = methods.First(x => x.Id == userMethodShowHistoryItem.Key);
            }

            var methodDto = new MethodDto
            {
                Id = methodToShow.Id,
                Title = methodToShow.Title,
                Text = methodToShow.Text,
                Type = methodToShow.Type.ToString(),
                ImageUrl = methodToShow.Image?.Url
            };

            if (methodToShow.Type == MethodType.YesNo)
            {
                var userMethod = await _context.UserMethods.Where(x => x.UserId == userId && x.MethodId == methodToShow.Id && x.Created > DateTime.Now.AddDays(-_statisticsPeriod)).OrderBy(x => x.Created).LastOrDefaultAsync(cancellationToken);

                if (userMethod?.Answer != null)
                {
                    methodDto.UserAnswer = userMethod.Answer;
                    methodDto.DateAnswered = (userMethod.LastModified ?? userMethod.Created).ToLocalTime().Date;
                    methodDto.TimeAnswered = (userMethod.LastModified ?? userMethod.Created).ToLocalTime().TimeOfDay;
                }
            }
            else
            {
                methodDto.Choices = new List<MethodMultipleChoiceDto>();

                var userMethods = (await _context.UserMethods.Where(x =>
                            x.UserId == userId && x.MethodId == methodToShow.Id &&
                            x.Created > DateTime.Now.AddDays(-_statisticsPeriod))
                        .ToListAsync(cancellationToken))
                    .GroupBy(g => g.MethodMultipleChoiceId)
                    .Select(x => x.OrderBy(y => y.Created).Last()).Select(x => x.MethodMultipleChoiceId.Value).ToList();

                if (userMethods.Any())
                {
                    var userMethod = await _context.UserMethods.Where(x => x.UserId == userId && x.MethodId == methodToShow.Id && x.Created > DateTime.Now.AddDays(-_statisticsPeriod)).OrderBy(x => x.Created).LastOrDefaultAsync(cancellationToken);

                    methodDto.DateAnswered = (userMethod.LastModified ?? userMethod.Created).ToLocalTime().Date;
                    methodDto.TimeAnswered = (userMethod.LastModified ?? userMethod.Created).ToLocalTime().TimeOfDay;
                }

                foreach (var methodMultipleChoice in await _context.MethodMultipleChoice
                    .Where(x => x.MethodId == methodToShow.Id).ToListAsync(cancellationToken))
                {
                    methodDto.Choices.Add(new MethodMultipleChoiceDto
                    {
                        Id = methodMultipleChoice.Id,
                        Title = methodMultipleChoice.Title,
                        IsCorrect = methodMultipleChoice.IsCorrect,
                        CheckedByUser = userMethods.Contains(methodMultipleChoice.Id)
                    });
                }
            }

            result.Add(methodDto);

            await _context.UserMethodShowHistory.AddAsync(new UserMethodShowHistoryItem { MethodId = methodToShow.Id, UserId = userId }, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);


            return result;
        }

        public async Task InsertAsync(string userId, InsertMethodPayload payload, CancellationToken cancellationToken)
        {
            foreach (var item in payload.Methods)
            {
                var method = await _context.Methods.SingleOrDefaultAsync(x => x.Id == item.Id, cancellationToken);

                if (method == null)
                {
                    throw new NotFoundException(nameof(Method), (item.Id));
                }

                if (method.Type == MethodType.YesNo)
                {
                    if (item.UserAnswer != null)
                    {
                        var userMethod = await _context.UserMethods.Where(x => x.UserId == userId && x.MethodId == method.Id && x.Created > DateTime.Now.AddDays(-_statisticsPeriod)).OrderBy(x => x.Created).LastOrDefaultAsync(cancellationToken);

                        if (userMethod == null)
                        {
                            await _context.UserMethods.AddAsync(new UserMethod { UserId = userId, MethodId = method.Id, Answer = item.UserAnswer }, cancellationToken);
                        }
                        else
                        {
                            userMethod.Answer = item.UserAnswer;
                            _context.UserMethods.Update(userMethod);
                        }
                    }
                }
                else
                {
                    foreach (var userMethod in (await _context.UserMethods.Where(x =>
                        x.UserId == userId && x.MethodId == method.Id && x.Created > DateTime.Now.AddDays(-_statisticsPeriod))
                        .ToListAsync(cancellationToken))
                        .GroupBy(g => g.MethodMultipleChoiceId)
                        .Select(x => x.OrderBy(y => y.Created).Last()))
                    {
                        _context.UserMethods.Remove(userMethod);
                    }

                    foreach (var choice in item.Choices)
                    {
                        await _context.UserMethods.AddAsync(new UserMethod { UserId = userId, MethodId = method.Id, MethodMultipleChoiceId = choice.Id }, cancellationToken);
                    }
                }

                await _context.SaveChangesAsync(cancellationToken);
            }
        }
    }
}
