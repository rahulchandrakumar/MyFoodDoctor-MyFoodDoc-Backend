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
using MyFoodDoc.App.Application.Payloads.Diary;
using MyFoodDoc.App.Application.Payloads.Method;
using MyFoodDoc.Application.Abstractions;
using MyFoodDoc.Application.Entities;
using MyFoodDoc.Application.Entities.Methods;
using MyFoodDoc.Application.Enums;

namespace MyFoodDoc.App.Application.Services
{
    public class MethodService : IMethodService
    {
        private readonly IApplicationContext _context;
        private readonly IUserHistoryService _userHistoryService;
        private readonly int _statisticsPeriod;

        public MethodService(IApplicationContext context, IUserHistoryService userHistoryService, IOptions<StatisticsOptions> statisticsOptions)
        {
            _context = context;
            _userHistoryService = userHistoryService;
            _statisticsPeriod = statisticsOptions.Value.Period > 0 ? statisticsOptions.Value.Period : 7;
        }

        public async Task<ICollection<MethodDto>> GetAsync(string userId, CancellationToken cancellationToken)
        {
            var result = new List<MethodDto>();

            var userDiets = await _context.UserDiets.Where(x => x.UserId == userId).Select(x => x.DietId).ToListAsync(cancellationToken);
            var userIndications = await _context.UserIndications.Where(x => x.UserId == userId).Select(x => x.IndicationId).ToListAsync(cancellationToken);
            var userMotivations = await _context.UserMotivations.Where(x => x.UserId == userId).Select(x => x.MotivationId).ToListAsync(cancellationToken);

            var availableMethodIds = _context.DietMethods.Where(x => userDiets.Contains(x.DietId)).Select(x => x.MethodId)
                .Union(_context.IndicationMethods.Where(x => userIndications.Contains(x.IndicationId)).Select(x => x.MethodId))
                .Union(_context.MotivationMethods.Where(x => userMotivations.Contains(x.MotivationId)).Select(x => x.MethodId)).Distinct();

            if (!availableMethodIds.Any())
                return result;

            var userTargetIds = (await _context.UserTargets.Where(x =>
                        x.UserId == userId)
                    .ToListAsync(cancellationToken))
                .GroupBy(g => g.TargetId).Select(x => x.OrderBy(y => y.Created).Last()).Select(x => x.TargetId).ToList();

            foreach (var method in await _context.Methods
                .Include(x => x.Targets)
                .Include(x => x.Image)
                .Where(x => availableMethodIds.Contains(x.Id))
                .ToListAsync(cancellationToken))
            {
                if (!method.Targets.Any() && method.Frequency == null && method.FrequencyPeriod == null)
                    continue;
                
                if (method.Targets.Any() && !method.Targets.Any(x =>  userTargetIds.Contains(x.TargetId)))
                    continue;
                
                if (method.Frequency != null && method.FrequencyPeriod != null)
                {
                    int daysInPeriod = 0;

                    switch (method.FrequencyPeriod.Value)
                    {
                        case MethodFrequencyPeriod.Day:
                            daysInPeriod = 1;
                            break;
                        case MethodFrequencyPeriod.Week:
                            daysInPeriod = 7;
                            break;
                        case MethodFrequencyPeriod.Month:
                            daysInPeriod = 30;
                            break;
                    }
                    
                    var checkPeriod = TimeSpan.FromDays(daysInPeriod).Divide(method.Frequency.Value);

                    if (method.Type == MethodType.Information)
                    {
                        if (result.Any(x => Enum.Parse<MethodType>(x.Type) == MethodType.Information) || 
                            (await _context.UserMethodShowHistory.Where(x => x.UserId == userId && x.MethodId == method.Id)
                                .ToListAsync(cancellationToken))
                            .Any(x => x.Date.ToLocalTime() > DateTime.Now.Subtract(checkPeriod)))
                            continue;
                    }
                    else
                    {
                        if ((await _context.UserMethods.Where(x => x.UserId == userId && x.MethodId == method.Id)
                                .ToListAsync(cancellationToken))
                            .Any(x => (x.LastModified ?? x.Created).ToLocalTime() > DateTime.Now.Subtract(checkPeriod)))
                            continue;

                        switch (method.Type)
                        {
                            case MethodType.AbdominalGirth:

                                if ((await _context.UserAbdominalGirths.Where(x => x.UserId == userId)
                                        .ToListAsync(cancellationToken))
                                    .Any(x => x.Date.ToLocalTime() > DateTime.Now.Subtract(checkPeriod)))
                                    continue;

                                break;
                            case MethodType.Mood:

                                if ((await _context.Meals.Where(x => x.UserId == userId && x.Mood != null)
                                        .ToListAsync(cancellationToken))
                                    .Any(x => x.Date.ToLocalTime() > DateTime.Now.Subtract(checkPeriod)))
                                    continue;

                                break;
                            case MethodType.Weight:

                                if ((await _context.UserWeights.Where(x => x.UserId == userId)
                                        .ToListAsync(cancellationToken))
                                    .Any(x => x.Date.ToLocalTime() > DateTime.Now.Subtract(checkPeriod)))
                                    continue;

                                break;
                        }
                    }
                }
                
                var methodDto = new MethodDto
                {
                    Id = method.Id,
                    Title = method.Title,
                    Text = method.Text,
                    Type = method.Type.ToString(),
                    ImageUrl = method.Image?.Url
                };

                var userMethod = await _context.UserMethods
                    .Where(x => x.UserId == userId && x.MethodId == method.Id &&
                                x.Created > DateTime.Now.AddDays(-_statisticsPeriod)).OrderBy(x => x.Created)
                    .LastOrDefaultAsync(cancellationToken);

                switch (method.Type)
                {
                    case MethodType.AbdominalGirth:

                        var userAbdominalGirth = await _context.UserAbdominalGirths.Where(x => x.UserId == userId)
                            .OrderBy(x => x.Date).LastOrDefaultAsync(cancellationToken);

                        if (userAbdominalGirth != null)
                        {
                            methodDto.UserAnswerDecimal = userAbdominalGirth.Value;
                            methodDto.DateAnswered = userAbdominalGirth.Date.ToLocalTime().Date;
                        }

                        break;
                    case MethodType.Change:
                    case MethodType.Drink:
                    case MethodType.Meals:
                    case MethodType.Sport:

                        if (userMethod?.Answer != null)
                        {
                            methodDto.UserAnswer = userMethod.Answer;
                            methodDto.DateAnswered = (userMethod.LastModified ?? userMethod.Created).ToLocalTime().Date;
                            methodDto.TimeAnswered =
                                (userMethod.LastModified ?? userMethod.Created).ToLocalTime().TimeOfDay;
                        }

                        break;
                    case MethodType.Knowledge:

                        methodDto.Choices = new List<MethodMultipleChoiceDto>();

                        var userMethods = (await _context.UserMethods.Where(x =>
                                    x.UserId == userId && x.MethodId == method.Id &&
                                    x.Created > DateTime.Now.AddDays(-_statisticsPeriod))
                                .ToListAsync(cancellationToken))
                            .GroupBy(g => g.MethodMultipleChoiceId)
                            .Select(x => x.OrderBy(y => y.Created).Last()).Select(x => x.MethodMultipleChoiceId.Value)
                            .ToList();

                        if (userMethods.Any())
                        {
                            methodDto.DateAnswered = (userMethod.LastModified ?? userMethod.Created).ToLocalTime().Date;
                            methodDto.TimeAnswered =
                                (userMethod.LastModified ?? userMethod.Created).ToLocalTime().TimeOfDay;
                        }

                        foreach (var methodMultipleChoice in await _context.MethodMultipleChoice
                            .Where(x => x.MethodId == method.Id).ToListAsync(cancellationToken))
                        {
                            methodDto.Choices.Add(new MethodMultipleChoiceDto
                            {
                                Id = methodMultipleChoice.Id,
                                Title = methodMultipleChoice.Title,
                                IsCorrect = methodMultipleChoice.IsCorrect,
                                CheckedByUser = userMethods.Contains(methodMultipleChoice.Id)
                            });
                        }

                        break;
                    case MethodType.Mood:

                        if (userMethod?.IntegerValue != null)
                        {
                            methodDto.UserAnswerInteger = userMethod.IntegerValue;
                            methodDto.DateAnswered = (userMethod.LastModified ?? userMethod.Created).ToLocalTime().Date;
                            methodDto.TimeAnswered =
                                (userMethod.LastModified ?? userMethod.Created).ToLocalTime().TimeOfDay;
                        }
                        else
                        {
                            var userMeal = await _context.Meals.Where(x => x.UserId == userId && x.Mood != null)
                                .OrderBy(x => x.Created).LastOrDefaultAsync(cancellationToken);

                            if (userMeal != null)
                            {
                                methodDto.UserAnswerInteger = userMeal.Mood;
                                methodDto.DateAnswered = userMeal.Date;
                                methodDto.TimeAnswered = userMeal.Time;
                            }
                        }

                        break;
                    case MethodType.Weight:

                        var userWeight = await _context.UserWeights.Where(x => x.UserId == userId)
                            .OrderBy(x => x.Date).LastOrDefaultAsync(cancellationToken);

                        if (userWeight != null)
                        {
                            methodDto.UserAnswerDecimal = userWeight.Value;
                            methodDto.DateAnswered = userWeight.Date.ToLocalTime().Date;
                        }

                        break;
                }

                result.Add(methodDto);

                await _context.UserMethodShowHistory.AddAsync(new UserMethodShowHistoryItem { MethodId = method.Id, UserId = userId }, cancellationToken);
            }

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
                
                switch (method.Type)
                {
                    case MethodType.AbdominalGirth:
                        if (item.DecimalValue != null)
                        {
                            await _userHistoryService.UpsertAbdonimalGirthHistoryAsync(userId,
                                new AbdominalGirthHistoryPayload { Date = DateTime.Now, Value = item.DecimalValue.Value }, cancellationToken);

                            await _context.UserMethods.AddAsync(new UserMethod { UserId = userId, MethodId = method.Id, DecimalValue = item.DecimalValue }, cancellationToken);
                        }

                        break;
                    case MethodType.Change:
                    case MethodType.Drink:
                    case MethodType.Meals:
                    case MethodType.Sport:

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

                        break;
                    case MethodType.Mood:

                        if (item.IntegerValue != null)
                        {
                            await _context.UserMethods.AddAsync(new UserMethod { UserId = userId, MethodId = method.Id, IntegerValue = item.IntegerValue }, cancellationToken);
                        }

                        break;
                    case MethodType.Knowledge:

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

                        break;
                    case MethodType.Weight:

                        if (item.DecimalValue != null)
                        {
                            await _userHistoryService.UpsertWeightHistoryAsync(userId,
                                new WeightHistoryPayload {Date = DateTime.Now, Value = item.DecimalValue.Value}, cancellationToken);

                            await _context.UserMethods.AddAsync(new UserMethod { UserId = userId, MethodId = method.Id, DecimalValue = item.DecimalValue }, cancellationToken);
                        }

                        break;
                }

                await _context.SaveChangesAsync(cancellationToken);
            }
        }
    }
}
