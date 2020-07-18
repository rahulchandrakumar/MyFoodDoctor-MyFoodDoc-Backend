using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
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
        private readonly ILogger<MethodService> _logger;

        public MethodService(IApplicationContext context, IUserHistoryService userHistoryService, ILogger<MethodService> logger)
        {
            _context = context;
            _userHistoryService = userHistoryService;
            _logger = logger;
        }

        public async Task<ICollection<MethodDto>> GetAsync(string userId, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Start");

            var result = new List<MethodDto>();

            if (!(await _context.Users.SingleAsync(x => x.Id == userId, cancellationToken)).HasSubscription)
                return result;

            _logger.LogInformation("Before User Indications");

            var userDiets = await _context.UserDiets.AsNoTracking()
                .Where(x => x.UserId == userId).Select(x => x.DietId).ToListAsync(cancellationToken);
            var userIndications = await _context.UserIndications.AsNoTracking()
                .Where(x => x.UserId == userId).Select(x => x.IndicationId).ToListAsync(cancellationToken);
            var userMotivations = await _context.UserMotivations.AsNoTracking()
                .Where(x => x.UserId == userId).Select(x => x.MotivationId).ToListAsync(cancellationToken);

            _logger.LogInformation("Before available methods");

            var dietMethods = userDiets.Any() ? await _context.DietMethods.AsNoTracking()
                .Where(x => userDiets.Contains(x.DietId))
                .Select(x => x.MethodId).ToListAsync(cancellationToken) : new List<int>();
            var indicationMethods = userIndications.Any() ? await _context.IndicationMethods.AsNoTracking()
                .Where(x => userIndications.Contains(x.IndicationId)).Select(x => x.MethodId)
                .ToListAsync(cancellationToken) : new List<int>();
            var motivationMethods = userMotivations.Any() ? await _context.MotivationMethods.AsNoTracking()
                .Where(x => userMotivations.Contains(x.MotivationId)).Select(x => x.MethodId)
                .ToListAsync(cancellationToken) : new List<int>();

            var availableMethodIds = dietMethods
                .Union(indicationMethods)
                .Union(motivationMethods).Distinct().ToList();

            if (!availableMethodIds.Any())
                return result;

            _logger.LogInformation("Before user targets");

            var userTargetIds = (await _context.UserTargets.AsNoTracking()
                    .Where(x =>
                        x.UserId == userId)
                    .ToListAsync(cancellationToken))
                .GroupBy(g => g.TargetId).Select(x => x.OrderBy(y => y.Created).Last()).Select(x => x.TargetId).ToList();

            _logger.LogInformation("Before user methods");

            foreach (var method in await _context.Methods
                .Include(x => x.Targets)
                .Include(x => x.Image)
                .AsNoTracking()
                .Where(x => availableMethodIds.Contains(x.Id))
                .ToListAsync(cancellationToken))
            {
                if (!method.Targets.Any() && method.Frequency == null && method.FrequencyPeriod == null)
                    continue;

                if (method.Targets.Any() && !method.Targets.Any(x => userTargetIds.Contains(x.TargetId)))
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
                        if ((await _context.UserMethods.AsNoTracking()
                                .Where(x => x.UserId == userId && x.MethodId == method.Id)
                                .ToListAsync(cancellationToken))
                            .Any(x => x.Created.ToLocalTime() > DateTime.Now.Subtract(checkPeriod)))
                            continue;

                        switch (method.Type)
                        {
                            case MethodType.AbdominalGirth:

                                if ((await _context.UserAbdominalGirths.AsNoTracking()
                                        .Where(x => x.UserId == userId)
                                        .ToListAsync(cancellationToken))
                                    .Any(x => x.Date.ToLocalTime() > DateTime.Now.Subtract(checkPeriod)))
                                    continue;

                                break;
                            case MethodType.Mood:

                                if ((await _context.Meals.AsNoTracking()
                                        .Where(x => x.UserId == userId && x.Mood != null)
                                        .ToListAsync(cancellationToken))
                                    .Any(x => x.Date.ToLocalTime() > DateTime.Now.Subtract(checkPeriod)))
                                    continue;

                                break;
                            case MethodType.Weight:

                                if ((await _context.UserWeights.AsNoTracking()
                                        .Where(x => x.UserId == userId)
                                        .ToListAsync(cancellationToken))
                                    .Any(x => x.Date.ToLocalTime() > DateTime.Now.Subtract(checkPeriod)))
                                    continue;

                                break;
                        }
                    }
                }

                _logger.LogInformation("Before GetMethodWithAnswersAsync " + method.Id);

                var methodDto = await GetMethodWithAnswersAsync(userId, method, DateTime.Now, cancellationToken);

                result.Add(methodDto);

                _logger.LogInformation("Method added " + method.Id);
            }

            if (result.Any())
            {
                _logger.LogInformation("Save");

                await _context.UserMethodShowHistory.AddRangeAsync(result.Select(x => new UserMethodShowHistoryItem { MethodId = x.Id, UserId = userId }), cancellationToken);
                await _context.SaveChangesAsync(cancellationToken);
            }

            _logger.LogInformation("Return");

            return result;
        }

        public async Task<ICollection<MethodDto>> GetByDateAsync(string userId, DateTime date,
            CancellationToken cancellationToken)
        {
            var result = new List<MethodDto>();

            foreach (var userMethod in (await _context.UserMethods
                    .Include(x => x.Method)
                    .ThenInclude(x => x.Image).AsNoTracking()
                .Where(x => x.UserId == userId)
                .ToListAsync(cancellationToken))
                .Where(x => x.Created.ToLocalTime().Date == date.Date)
                .GroupBy(k => k.MethodId)
                .Select(g => g.First()))
            {
                var methodDto = await GetMethodWithAnswersAsync(userId, userMethod.Method, date, cancellationToken);

                result.Add(methodDto);
            }

            foreach (var userMethod in (await _context.UserMethodShowHistory
                .Include(x => x.Method)
                .ThenInclude(x => x.Image)
                .Where(x => x.UserId == userId && x.Method.Type == MethodType.Information)
                .ToListAsync(cancellationToken))
                .Where(x => x.Date.ToLocalTime().Date == date.Date)
                .GroupBy(k => k.MethodId).Select(g => g.First()))
            {
                result.Add(new MethodDto
                {
                    Id = userMethod.Method.Id,
                    Title = userMethod.Method.Title,
                    Text = userMethod.Method.Text,
                    Type = userMethod.Method.Type.ToString(),
                    ImageUrl = userMethod.Method.Image?.Url
                });
            }

            return result;
        }

        private async Task<MethodDto> GetMethodWithAnswersAsync(string userId, Method method, DateTime date, CancellationToken cancellationToken)
        {
            var result = new MethodDto
            {
                Id = method.Id,
                Title = method.Title,
                Text = method.Text,
                Type = method.Type.ToString(),
                ImageUrl = method.Image?.Url
            };

            _logger.LogInformation("Before UserMethods " + method.Id);

            var userMethodsOnDate = (await _context.UserMethods.AsNoTracking()
                .Where(x =>
                    x.UserId == userId && x.MethodId == method.Id).ToListAsync(cancellationToken)).Where(x=>
                    x.Created.ToLocalTime().Date == date.Date).ToList();

            var userMethod = userMethodsOnDate.OrderBy(x => x.Created).LastOrDefault();

            _logger.LogInformation("After UserMethods " + method.Id);

            switch (method.Type)
            {
                case MethodType.AbdominalGirth:

                    var userAbdominalGirth = await _context.UserAbdominalGirths.AsNoTracking()
                        .Where(x => x.UserId == userId && x.Date.Date == date.Date)
                        .OrderBy(x => x.Date).LastOrDefaultAsync(cancellationToken);

                    if (userAbdominalGirth != null)
                    {
                        result.UserAnswerDecimal = userAbdominalGirth.Value;
                        result.DateAnswered = userAbdominalGirth.Date.ToLocalTime().Date;
                    }

                    break;
                case MethodType.Change:
                case MethodType.Drink:
                case MethodType.Meals:
                case MethodType.Sport:

                    if (userMethod?.Answer != null)
                    {
                        result.UserAnswerBoolean = userMethod.Answer;
                        result.DateAnswered = (userMethod.LastModified ?? userMethod.Created).ToLocalTime().Date;
                        result.TimeAnswered =
                            (userMethod.LastModified ?? userMethod.Created).ToLocalTime().TimeOfDay;
                    }

                    break;
                case MethodType.Knowledge:

                    result.Choices = new List<MethodMultipleChoiceDto>();

                    var userMethodChoices = userMethodsOnDate
                        .GroupBy(g => g.MethodMultipleChoiceId)
                        .Select(x => x.OrderBy(y => y.Created).Last()).Select(x => x.MethodMultipleChoiceId.Value)
                        .ToList();

                    if (userMethodChoices.Any())
                    {
                        result.DateAnswered = (userMethod.LastModified ?? userMethod.Created).ToLocalTime().Date;
                        result.TimeAnswered =
                            (userMethod.LastModified ?? userMethod.Created).ToLocalTime().TimeOfDay;
                    }

                    foreach (var methodMultipleChoice in await _context.MethodMultipleChoice.AsNoTracking()
                        .Where(x => x.MethodId == method.Id).ToListAsync(cancellationToken))
                    {
                        result.Choices.Add(new MethodMultipleChoiceDto
                        {
                            Id = methodMultipleChoice.Id,
                            Title = methodMultipleChoice.Title,
                            IsCorrect = methodMultipleChoice.IsCorrect,
                            CheckedByUser = userMethodChoices.Contains(methodMultipleChoice.Id)
                        });
                    }

                    break;
                case MethodType.Mood:

                    var userMeal = await _context.Meals.AsNoTracking()
                        .Where(x => x.UserId == userId && x.Mood != null && x.Date.Date == date.Date)
                        .OrderBy(x => x.Created).LastOrDefaultAsync(cancellationToken);

                    if (userMethod?.IntegerValue != null && userMeal != null)
                    {
                        if ((userMethod.LastModified ?? userMethod.Created).ToLocalTime().TimeOfDay > userMeal.Time)
                        {
                            result.UserAnswerInteger = userMethod.IntegerValue;
                            result.DateAnswered = (userMethod.LastModified ?? userMethod.Created).ToLocalTime().Date;
                            result.TimeAnswered =
                                (userMethod.LastModified ?? userMethod.Created).ToLocalTime().TimeOfDay;
                        }
                        else
                        {
                            result.UserAnswerInteger = userMeal.Mood;
                            result.DateAnswered = userMeal.Date;
                            result.TimeAnswered = userMeal.Time;
                        }
                    }
                    else if (userMethod?.IntegerValue != null)
                    {
                        result.UserAnswerInteger = userMethod.IntegerValue;
                        result.DateAnswered = (userMethod.LastModified ?? userMethod.Created).ToLocalTime().Date;
                        result.TimeAnswered =
                            (userMethod.LastModified ?? userMethod.Created).ToLocalTime().TimeOfDay;
                    }
                    else if (userMeal != null)
                    {
                        result.UserAnswerInteger = userMeal.Mood;
                        result.DateAnswered = userMeal.Date;
                        result.TimeAnswered = userMeal.Time;
                    }

                    break;
                case MethodType.Weight:

                    var userWeight = await _context.UserWeights.AsNoTracking()
                        .Where(x => x.UserId == userId && x.Date.Date == date.Date)
                        .OrderBy(x => x.Date).LastOrDefaultAsync(cancellationToken);

                    if (userWeight != null)
                    {
                        result.UserAnswerDecimal = userWeight.Value;
                        result.DateAnswered = userWeight.Date.ToLocalTime().Date;
                    }

                    break;
            }

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
                        if (item.UserAnswerDecimal != null)
                        {
                            await _userHistoryService.UpsertAbdominalGirthHistoryAsync(userId,
                                new AbdominalGirthHistoryPayload { Date = DateTime.Now, Value = item.UserAnswerDecimal.Value }, cancellationToken);

                            await _context.UserMethods.AddAsync(new UserMethod { UserId = userId, MethodId = method.Id, DecimalValue = item.UserAnswerDecimal }, cancellationToken);
                        }

                        break;
                    case MethodType.Change:
                    case MethodType.Drink:
                    case MethodType.Meals:
                    case MethodType.Sport:

                        if (item.UserAnswerBoolean != null)
                        {
                            var userMethod = (await _context.UserMethods.Where(x => x.UserId == userId && x.MethodId == method.Id).ToListAsync(cancellationToken)).Where(x => x.Created.ToLocalTime().Date == DateTime.Now.Date).OrderBy(x => x.Created).LastOrDefault();

                            if (userMethod == null)
                            {
                                await _context.UserMethods.AddAsync(new UserMethod { UserId = userId, MethodId = method.Id, Answer = item.UserAnswerBoolean }, cancellationToken);
                            }
                            else
                            {
                                userMethod.Answer = item.UserAnswerBoolean;
                                _context.UserMethods.Update(userMethod);
                            }
                        }

                        break;
                    case MethodType.Mood:

                        if (item.UserAnswerInteger != null)
                        {
                            await _context.UserMethods.AddAsync(new UserMethod { UserId = userId, MethodId = method.Id, IntegerValue = item.UserAnswerInteger }, cancellationToken);
                        }

                        break;
                    case MethodType.Knowledge:

                        foreach (var userMethod in (await _context.UserMethods.Where(x =>
                                    x.UserId == userId && x.MethodId == method.Id)
                                .ToListAsync(cancellationToken))
                            .Where(x=> x.Created.ToLocalTime().Date == DateTime.Now.Date)
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

                        if (item.UserAnswerDecimal != null)
                        {
                            await _userHistoryService.UpsertWeightHistoryAsync(userId,
                                new WeightHistoryPayload { Date = DateTime.UtcNow, Value = item.UserAnswerDecimal.Value }, cancellationToken);

                            await _context.UserMethods.AddAsync(new UserMethod { UserId = userId, MethodId = method.Id, DecimalValue = item.UserAnswerDecimal }, cancellationToken);
                        }

                        break;
                }

                await _context.SaveChangesAsync(cancellationToken);
            }
        }
    }
}
