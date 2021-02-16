using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MyFoodDoc.App.Application.Abstractions;
using MyFoodDoc.App.Application.Exceptions;
using MyFoodDoc.App.Application.Models;
using MyFoodDoc.App.Application.Payloads.Diary;
using MyFoodDoc.App.Application.Payloads.Method;
using MyFoodDoc.Application.Abstractions;
using MyFoodDoc.Application.Configuration;
using MyFoodDoc.Application.Entities;
using MyFoodDoc.Application.Entities.Methods;
using MyFoodDoc.Application.EnumEntities;
using MyFoodDoc.Application.Enums;

namespace MyFoodDoc.App.Application.Services
{
    public class MethodService : IMethodService
    {
        private const int MAX_METHODS_PER_OPTIMIZATION_AREA = 3;

        private readonly IApplicationContext _context;
        private readonly IUserHistoryService _userHistoryService;
        private readonly ILogger<MethodService> _logger;
        private readonly int _statisticsPeriod;

        public MethodService(IApplicationContext context, IUserHistoryService userHistoryService, ILogger<MethodService> logger, IOptions<StatisticsOptions> statisticsOptions)
        {
            _context = context;
            _userHistoryService = userHistoryService;
            _statisticsPeriod = statisticsOptions.Value.Period > 0 ? statisticsOptions.Value.Period : 7;
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<ICollection<MethodDto>> GetAsync(string userId, DateTime date, CancellationToken cancellationToken)
        {
            _logger.LogInformation($"START MethodService.GetAsync. userId={userId}, date={date}, localDate={date.ToLocalTime()}.");

            var result = new List<MethodDto>();

            var user = await _context.Users.SingleOrDefaultAsync(x => x.Id == userId, cancellationToken);

            if (user == null)
            {
                throw new NotFoundException(nameof(User), userId);
            }

            if (user.HasValidSubscription == null || !user.HasValidSubscription.Value)
                return result;

            var userMethodShowHistory = await _context.UserMethodShowHistory
                .Include(x => x.Method)
                .ThenInclude(x => x.Parent)
                .Where(x => x.UserId == userId)
                .ToListAsync(cancellationToken);

            foreach (var umsh in userMethodShowHistory)
            {
                _logger.LogInformation($"UserMethodShowHistory. dateTime={umsh}, localDateTime={umsh.Date.ToLocalTime()}, localDate={umsh.Date.ToLocalTime().Date}, dateDate={date.Date}, check={umsh.Date.ToLocalTime().Date == date.Date}.");
            }

            //Check history on date
            if (userMethodShowHistory.Any(x => x.Date.ToLocalTime().Date == date.Date))
                return await GetByDateAsync(userId, date, cancellationToken);

            var userDiets = await _context.UserDiets.AsNoTracking()
                .Where(x => x.UserId == userId).Select(x => x.DietId).ToListAsync(cancellationToken);
            var userIndications = await _context.UserIndications.AsNoTracking()
                .Where(x => x.UserId == userId).Select(x => x.IndicationId).ToListAsync(cancellationToken);
            var userMotivations = await _context.UserMotivations.AsNoTracking()
                .Where(x => x.UserId == userId).Select(x => x.MotivationId).ToListAsync(cancellationToken);

            var userTargets = await _context.UserTargets.AsNoTracking()
                .Where(x =>
                    x.UserId == userId)
                .ToListAsync(cancellationToken);

            var lastUserTarget = userTargets.Where(x => x.Created.ToLocalTime().Date <= date.Date)
                .OrderBy(x => x.Created)
                .LastOrDefault();

            var userTargetIds = lastUserTarget != null ? userTargets.Where(x => x.Created.Date == lastUserTarget.Created.Date)
                .GroupBy(g => g.TargetId).Select(x => x.OrderBy(y => y.Created).Last()).Select(x => x.TargetId).ToList() : new List<int>();

            var userMethods = await _context.UserMethods.AsNoTracking()
                .Include(x => x.Method)
                .Where(x =>
                    x.UserId == userId)
                .ToListAsync(cancellationToken);

            List<Method> childMethodsToShow = new List<Method>();
            List<Method> parentFrequencyMethodsToShow = new List<Method>();
            List<MethodDto> parentMethodsToShow = new List<MethodDto>();
            List<MethodDto> timerMethodsToShow = new List<MethodDto>();
            
            foreach (var method in await _context.Methods
                .Include(x => x.Targets)
                .Include(x => x.Diets)
                .Include(x => x.Indications)
                .Include(x => x.Motivations)
                .Include(x => x.Image)
                .Include(x => x.Children)
                .ThenInclude(x => x.Image)
                .AsNoTracking()
                .Where(x => x.IsActive && x.ParentId == null)
                .ToListAsync(cancellationToken))
            {
                //Frequency-based or target-based method check
                if (!method.Targets.Any() && method.Frequency == null && method.FrequencyPeriod == null)
                    continue;

                //Contraindications check
                if (method.Diets.Any(x => x.IsContraindication && userDiets.Contains(x.DietId)) ||
                    method.Indications.Any(x => x.IsContraindication && userIndications.Contains(x.IndicationId)) ||
                    method.Motivations.Any(x => x.IsContraindication && userMotivations.Contains(x.MotivationId)))
                    continue;

                //Diets, indications and motivations check
                if (!method.Diets.Any(x => !x.IsContraindication && userDiets.Contains(x.DietId)) &&
                    !method.Indications.Any(x => !x.IsContraindication && userIndications.Contains(x.IndicationId)) &&
                    !method.Motivations.Any(x => !x.IsContraindication && userMotivations.Contains(x.MotivationId)))
                    continue;

                //Target-based method check
                if (method.Targets.Any() && !method.Targets.Any(x => userTargetIds.Contains(x.TargetId)))
                    continue;

                //Frequency-based method check
                if (method.Frequency != null && method.FrequencyPeriod != null && !(method.Type == MethodType.Change || method.Type == MethodType.Drink || method.Type == MethodType.Meals))
                {
                    if (!await CheckFrequency(userId, method, date, userMethods, userMethodShowHistory, cancellationToken))
                        continue;
                }

                if ((method.Type == MethodType.Change || method.Type == MethodType.Drink || method.Type == MethodType.Meals))
                {
                    var userMethod = userMethods
                            .Where(x => x.MethodId == method.Id && (lastUserTarget == null || x.Created >= lastUserTarget.Created) && x.Created.ToLocalTime().Date <= date.Date).OrderBy(x => x.LastModified ?? x.Created)
                            .LastOrDefault();

                    if (userMethod == null && method.Frequency != null && method.FrequencyPeriod != null && !await CheckFrequency(userId, method, date, userMethods, userMethodShowHistory, cancellationToken))
                        continue;

                    //Child methods check
                    if (method.Children.Any(x => x.IsActive) && userMethod?.Answer != null && userMethod.Answer.Value)
                    {
                        foreach (var childMethod in method.Children.Where(x => x.IsActive))
                        {
                            //Frequency-based method check
                            if (childMethod.Frequency != null && childMethod.FrequencyPeriod != null)
                            {
                                if (!await CheckFrequency(userId, childMethod, date, userMethods, userMethodShowHistory, cancellationToken))
                                    continue;

                                childMethodsToShow.Add(childMethod);
                            }
                        }
                    }
                }

                if (method.Type == MethodType.Change || method.Type == MethodType.Drink ||
                    method.Type == MethodType.Meals)
                {
                    var methodDto = await GetMethodWithAnswersAsync(userId, method, date, lastUserTarget, userMethods, cancellationToken);

                    parentMethodsToShow.Add(methodDto);
                }
                else if (method.Type == MethodType.Timer)
                {
                    var methodDto = await GetMethodWithAnswersAsync(userId, method, date, lastUserTarget, userMethods, cancellationToken);

                    timerMethodsToShow.Add(methodDto);
                }
                else
                {
                    parentFrequencyMethodsToShow.Add(method);
                }
            }

            var userMethodShowHistoryForPeriod = userMethodShowHistory.Where(x => (lastUserTarget == null || x.Date.ToLocalTime().Date >= lastUserTarget.Created.ToLocalTime().Date) && x.Date.ToLocalTime().Date <= date.Date).ToList();

            if (childMethodsToShow.Any())
            {
                Method methodToShow = null;

                if (userMethodShowHistoryForPeriod.Count(x => x.Method.Type == MethodType.Information ||
                                                              x.Method.Type == MethodType.Knowledge) < _statisticsPeriod - 1)
                {
                    var groupedHistory = userMethodShowHistory
                        .Where(x => childMethodsToShow.Any(y => y.Id == x.MethodId))
                        .GroupBy(k => k.MethodId)
                        .Select(g => new { MethodId = g.Key, Count = g.Count() }).ToList();

                    var childMethodsToShowOrdered = childMethodsToShow.OrderBy(x => groupedHistory.FirstOrDefault(y => y.MethodId == x.Id)?.Count ?? 0).ToList();

                    //New parent method type
                    methodToShow = childMethodsToShowOrdered.FirstOrDefault(x =>
                        !userMethodShowHistoryForPeriod.Any(
                            y => y.Method.Parent != null && y.Method.Parent.Type == x.Parent.Type));

                    if (methodToShow == null)
                    {
                        //New parent method
                        methodToShow = childMethodsToShowOrdered.FirstOrDefault(x =>
                            !userMethodShowHistoryForPeriod.Any(y =>
                                y.Method.Parent != null && y.Method.ParentId == x.ParentId));

                        if (methodToShow == null)
                        {
                            //New method
                            methodToShow = childMethodsToShowOrdered.FirstOrDefault(x =>
                                !userMethodShowHistoryForPeriod.Any(y => y.MethodId == x.Id));
                        }
                    }
                }

                if (methodToShow == null)
                {
                    //Less shown method
                    var methodIdToShow = userMethodShowHistoryForPeriod
                        .Where(x => childMethodsToShow.Any(y => y.Id == x.MethodId))
                        .GroupBy(k => k.MethodId)
                        .Select(g => new { MethodId = g.Key, Count = g.Count() })
                        .OrderBy(x => x.Count).First().MethodId;

                    methodToShow = childMethodsToShow.First(x => x.Id == methodIdToShow);
                }

                var methodDto = await GetMethodWithAnswersAsync(userId, methodToShow, date, lastUserTarget, userMethods, cancellationToken);

                result.Add(methodDto);
            }

            if (parentFrequencyMethodsToShow.Any())
            {
                //New method type
                Method methodToShow = parentFrequencyMethodsToShow.FirstOrDefault(x => !userMethodShowHistoryForPeriod.Any(y => y.Method.Type == x.Type));

                if (methodToShow == null)
                {
                    //New method
                    methodToShow =
                        parentFrequencyMethodsToShow.FirstOrDefault(x =>
                            !userMethodShowHistoryForPeriod.Any(y => y.MethodId == x.Id));

                    if (methodToShow == null)
                    {
                        //Less shown method
                        var methodIdToShow = userMethodShowHistoryForPeriod
                            .Where(x => parentFrequencyMethodsToShow.Any(y => y.Id == x.MethodId))
                            .GroupBy(k => k.MethodId)
                            .Select(g => new { MethodId = g.Key, Count = g.Count() })
                            .OrderBy(x=> x.Count).First().MethodId;

                        methodToShow = parentFrequencyMethodsToShow.First(x => x.Id == methodIdToShow);
                    }
                }

                var methodDto = await GetMethodWithAnswersAsync(userId, methodToShow, date, lastUserTarget, userMethods, cancellationToken);

                result.Add(methodDto);
            }

            if (parentMethodsToShow.Any())
            {
                foreach (var group in parentMethodsToShow.GroupBy(g => g.OptimizationAreaKey))
                {
                    var answeredMethod =
                        group.FirstOrDefault(x => x.UserAnswerBoolean != null && x.UserAnswerBoolean.Value);

                    if (answeredMethod != null)
                        result.Add(answeredMethod);
                    else
                    {
                        result.AddRange(group.Take(MAX_METHODS_PER_OPTIMIZATION_AREA));
                    }
                }
            }

            //Delete running timer for the previous period if different
            if (lastUserTarget != null)
            {
                var previousTimerMethod = userMethods
                    .Where(x => x.Method.Type == MethodType.Timer && x.Created < lastUserTarget.Created)
                    .OrderBy(x => x.LastModified ?? x.Created)
                    .LastOrDefault();

                if (previousTimerMethod?.Answer != null &&
                    previousTimerMethod.Answer.Value)
                {
                    if (!timerMethodsToShow.Any(x => x.Id == previousTimerMethod.MethodId))
                    {
                        previousTimerMethod.Answer = false;

                        _context.UserMethods.Update(previousTimerMethod);

                        await _context.SaveChangesAsync(cancellationToken);

                        var userTimer = await _context.UserTimer
                            .Where(x => x.UserId == userId && x.MethodId == previousTimerMethod.MethodId)
                            .FirstOrDefaultAsync(cancellationToken);

                        if (userTimer != null)
                        {
                            _context.UserTimer.Remove(userTimer);

                            await _context.SaveChangesAsync(cancellationToken);
                        }
                    }
                }
            }

            if (timerMethodsToShow.Any())
            {
                result.AddRange(timerMethodsToShow);
            }

            if (result.Any())
            {
                await _context.UserMethodShowHistory.AddRangeAsync(result.Select(x => new UserMethodShowHistoryItem { MethodId = x.Id, UserId = userId, Date = date}), cancellationToken);
                await _context.SaveChangesAsync(cancellationToken);
            }

            return result;
        }

        private async Task<OptimizationArea> GetOptimizationAreaByMethodId(int methodId,
            CancellationToken cancellationToken)
        {
            var targetMethod = await _context.TargetMethods.FirstOrDefaultAsync(x => x.MethodId == methodId, cancellationToken);

            if (targetMethod == null)
                return null;

            var optimizationArea = (await _context.Targets
                .Include(x=> x.OptimizationArea)
                .SingleAsync(x => x.Id == targetMethod.TargetId, cancellationToken)).OptimizationArea;

            return optimizationArea;
        }

        private async Task<bool> CheckFrequency(string userId, Method method, DateTime date, List<UserMethod> userMethods, List<UserMethodShowHistoryItem> userMethodShowHistory, CancellationToken cancellationToken)
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
                if (userMethodShowHistory.Any(x => x.MethodId == method.Id && x.Date.ToLocalTime() > date.Subtract(checkPeriod)))
                    return false;
            }
            else
            {
                if (userMethods.Any(x => x.MethodId == method.Id && x.Created.ToLocalTime() > date.Subtract(checkPeriod)))
                    return false;

                switch (method.Type)
                {
                    case MethodType.AbdominalGirth:

                        if ((await _context.UserAbdominalGirths.AsNoTracking()
                                .Where(x => x.UserId == userId)
                                .ToListAsync(cancellationToken))
                            .Any(x => x.Date.ToLocalTime() > date.Subtract(checkPeriod)))
                            return false;

                        break;
                    case MethodType.Mood:

                        if ((await _context.Meals.AsNoTracking()
                                .Where(x => x.UserId == userId && x.Mood != null)
                                .ToListAsync(cancellationToken))
                            .Any(x => x.Date.ToLocalTime() > date.Subtract(checkPeriod)))
                            return false;

                        break;
                    case MethodType.Weight:

                        if ((await _context.UserWeights.AsNoTracking()
                                .Where(x => x.UserId == userId)
                                .ToListAsync(cancellationToken))
                            .Any(x => x.Date.ToLocalTime() > date.Subtract(checkPeriod)))
                            return false;

                        break;
                }
            }

            return true;
        }

        public async Task<ICollection<MethodDto>> GetByDateAsync(string userId, DateTime date,
            CancellationToken cancellationToken)
        {
            var result = new List<MethodDto>();

            var lastUserTarget = (await _context.UserTargets.AsNoTracking()
                    .Where(x =>
                        x.UserId == userId)
                    .ToListAsync(cancellationToken))
                .Where(x => x.Created.ToLocalTime().Date <= date.Date)
                .OrderBy(x => x.Created)
                .LastOrDefault();

            var userMethods = await _context.UserMethods.AsNoTracking()
                .Where(x =>
                    x.UserId == userId)
                .ToListAsync(cancellationToken);

            foreach (var userMethod in (await _context.UserMethodShowHistory
                    .Include(x => x.Method)
                    .ThenInclude(x => x.Image)
                    .Where(x => x.UserId == userId)
                    .ToListAsync(cancellationToken))
                .Where(x => x.Date.ToLocalTime().Date == date.Date)
                .GroupBy(k => k.MethodId).Select(g => g.First()))
            {
                var methodDto = await GetMethodWithAnswersAsync(userId, userMethod.Method, date, lastUserTarget, userMethods, cancellationToken);

                result.Add(methodDto);
            }

            return result;
        }

        private async Task<MethodDto> GetMethodWithAnswersAsync(string userId, Method method, DateTime date, UserTarget lastUserTarget, List<UserMethod> userMethods, CancellationToken cancellationToken)
        {
            var result = new MethodDto
            {
                Id = method.Id,
                Title = method.Title,
                Text = method.Text,
                Type = method.Type.ToString(),
                ImageUrl = method.Image?.Url,
                ParentId = method.ParentId
            };

            UserMethod userMethod = null;
            List<UserMethod> userMethodHistory = new List<UserMethod>();

            if (method.Type == MethodType.Change || method.Type == MethodType.Drink ||
                 method.Type == MethodType.Meals)
            {
                if (lastUserTarget != null)
                {
                    userMethodHistory = userMethods
                        .Where(x => x.MethodId == method.Id && x.Created >= lastUserTarget.Created && x.Created.ToLocalTime().Date <= date.Date)
                        .ToList();

                    userMethod = userMethodHistory.OrderBy(x => x.Created).LastOrDefault();
                }

                var optimizationArea = await GetOptimizationAreaByMethodId(method.Id, cancellationToken);

                result.OptimizationAreaKey = optimizationArea?.Key;
            }
            else if (method.Type == MethodType.Timer)
            {
                userMethodHistory = userMethods
                    .Where(x => x.MethodId == method.Id)
                    .ToList();

                userMethod = userMethodHistory.OrderBy(x => x.Created).LastOrDefault();
            }
            else
            {
                userMethodHistory = userMethods
                    .Where(x => x.MethodId == method.Id && x.Created.ToLocalTime().Date == date.Date)
                    .ToList();

                userMethod = userMethodHistory.OrderBy(x => x.Created).LastOrDefault();
            }

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

                    var userMethodChoices = userMethodHistory
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
                case MethodType.Timer:

                    //Method was answered today or timer is still running since yesterday
                    if (userMethod != null && (userMethod.Created.ToLocalTime().Date == date.Date || (userMethod.Answer != null && userMethod.Answer.Value)))
                    {
                        if (userMethod?.Answer != null && userMethod?.IntegerValue != null)
                        {
                            result.UserAnswerBoolean = userMethod.Answer;
                            result.UserAnswerInteger = userMethod.IntegerValue;
                            result.DateAnswered = (userMethod.LastModified ?? userMethod.Created).ToLocalTime().Date;
                            result.TimeAnswered =
                                (userMethod.LastModified ?? userMethod.Created).ToLocalTime().TimeOfDay;
                        }
                    }

                    result.TimeIntervalDay = method.TimeIntervalDay;
                    result.TimeIntervalNight = method.TimeIntervalNight;

                    result.Texts = await _context.MethodTexts.AsNoTracking()
                        .Where(x => x.MethodId == method.Id).Select(x => new MethodTextDto { Code = x.Code, Title = x.Title, Text = x.Text }).ToListAsync(cancellationToken);

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
                    case MethodType.Timer:
                        if (item.UserAnswerBoolean != null && item.UserAnswerInteger != null)
                        {
                            var userMethod = (await _context.UserMethods.Where(x => x.UserId == userId && x.MethodId == method.Id).ToListAsync(cancellationToken)).OrderBy(x => x.Created).LastOrDefault();
                            
                            //Method was answered today or timer is still running since yesterday
                            if (userMethod != null && (userMethod.Created.ToLocalTime().Date == DateTime.Now.Date ||
                                                       (userMethod.Answer != null && userMethod.Answer.Value)))
                            {
                                userMethod.Answer = item.UserAnswerBoolean;
                                userMethod.IntegerValue = item.UserAnswerInteger;
                                _context.UserMethods.Update(userMethod);
                            }
                            else
                            {
                                await _context.UserMethods.AddAsync(new UserMethod { UserId = userId, MethodId = method.Id, Answer = item.UserAnswerBoolean, IntegerValue = item.UserAnswerInteger }, cancellationToken);
                            }
                            
                            var userTimer = await _context.UserTimer.Where(x => x.UserId == userId)
                                .SingleOrDefaultAsync(cancellationToken);
                              
                            if (item.UserAnswerBoolean.Value)
                            {
                                var expirationDate = DateTime.Now.AddMinutes(item.UserAnswerInteger.Value);

                                if (userTimer == null)
                                {
                                    await _context.UserTimer.AddAsync(new UserTimer { UserId = userId, ExpirationDate = expirationDate, MethodId = method.Id }, cancellationToken);
                                }
                                else
                                {
                                    userTimer.ExpirationDate = expirationDate;

                                    _context.UserTimer.Update(userTimer);
                                }
                            }
                            else if (userTimer != null)
                            {
                                _context.UserTimer.Remove(userTimer);
                            }
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
