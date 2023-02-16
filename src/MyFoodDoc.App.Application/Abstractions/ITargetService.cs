using MyFoodDoc.App.Application.Models;
using MyFoodDoc.App.Application.Payloads.Target;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace MyFoodDoc.App.Application.Abstractions
{
    public interface ITargetService
    {
        Task<bool> NewTriggered(string userId, DateTime date, CancellationToken cancellationToken);
        Task<bool> AnyAnswered(string userId, CancellationToken cancellationToken);
        Task<bool> AnyActivated(string userId, DateTime date, CancellationToken cancellationToken);
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
