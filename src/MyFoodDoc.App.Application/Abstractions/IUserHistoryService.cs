using MyFoodDoc.App.Application.Models;
using MyFoodDoc.App.Application.Payloads.Diary;
using MyFoodDoc.App.Application.Payloads.User;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace MyFoodDoc.App.Application.Abstractions
{
    public interface IUserHistoryService
    {
        Task<UserHistoryDto> GetAggregationAsync(string userId, CancellationToken cancellationToken = default);

        Task<UserHistoryDtoWeight> GetWeightHistoryAsync(string userId, CancellationToken cancellationToken);

        Task UpsertWeightHistoryAsync(string userId, WeightHistoryPayload payload, CancellationToken cancellationToken = default);

        Task<UserHistoryDtoAbdominalGirth> GetAbdominalGirthHistoryAsync(string userId, CancellationToken cancellationToken);

        Task UpsertAbdominalGirthHistoryAsync(string userId, AbdominalGirthHistoryPayload payload, CancellationToken cancellationToken = default);
    }
}
