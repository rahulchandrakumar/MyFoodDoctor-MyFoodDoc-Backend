using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using MyFoodDoc.App.Application.Models;
using MyFoodDoc.App.Application.Payloads.Target;

namespace MyFoodDoc.App.Application.Abstractions.V2
{
    public interface ITargetServiceV2
    {
        Task<bool> NewTriggered(string userId, CancellationToken cancellationToken);
        Task<bool> AnyAnswered(string userId, CancellationToken cancellationToken);
        Task<bool> AnyActivated(string userId, CancellationToken cancellationToken);
        Task<int> GetDaysTillFirstEvaluationAsync(string userId, CancellationToken cancellationToken);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="onDate">If the argument is null, the last minute of the previous date will be used</param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<ICollection<OptimizationAreaDto>> GetAsync(string userId, DateTime? onDate, CancellationToken cancellationToken);
        Task<ICollection<OptimizationAreaDto>> GetLastAsync(string userId, CancellationToken cancellationToken);
        Task InsertAsync(string userId, InsertTargetPayload payload, CancellationToken cancellationToken);
    }
}
