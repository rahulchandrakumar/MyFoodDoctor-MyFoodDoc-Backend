using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using MyFoodDoc.App.Application.Models;
using MyFoodDoc.App.Application.Models.StatisticsDto;
using MyFoodDoc.App.Application.Payloads.Target;

namespace MyFoodDoc.App.Application.Abstractions.V2
{
    public interface ITargetServiceV2
    {
        bool NewTriggered(StatisticsUserDto statisticsUserDto, string userId);
        bool AnyAnswered(IEnumerable<UserTargetDto> userTargetDtos);
        bool AnyActivated(IEnumerable<UserTargetDto> userTargetDtos);
        int GetDaysTillFirstEvaluationAsync(StatisticsUserDto user, CancellationToken cancellationToken);
     
        IEnumerable<OptimizationAreaDto> Get(StatisticsUserDto user,
         string userId, DateTime? onDate);
        ICollection<OptimizationAreaDto> GetLastAsync(StatisticsUserDto user);
        Task InsertAsync(string userId, InsertTargetPayload payload, CancellationToken cancellationToken);
    }
}
