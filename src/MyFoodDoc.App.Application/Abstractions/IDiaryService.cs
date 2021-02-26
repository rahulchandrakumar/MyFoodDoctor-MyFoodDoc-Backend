using MyFoodDoc.App.Application.Models;
using MyFoodDoc.App.Application.Payloads.Diary;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace MyFoodDoc.App.Application.Abstractions
{
    public interface IDiaryService
    {
        Task<DiaryEntryDto> GetAggregationByDateAsync(string userId, DateTime start, CancellationToken cancellationToken = default);
        Task<DiaryEntryDtoMeal> GetMealAsync(string userId, int id, CancellationToken cancellationToken);
        Task<int> InsertMealAsync(string userId, InsertMealPayload payload, CancellationToken cancellationToken);
        Task<int> UpdateMealAsync(string userId, int id, UpdateMealPayload payload, CancellationToken cancellationToken);
        Task RemoveMealAsync(string userId, int id, CancellationToken cancellationToken);
        Task<DiaryEntryDtoLiquid> GetLiquidAsync(string userId, DateTime date, CancellationToken cancellationToken);
        Task UpsertLiquidAsync(string userId, LiquidPayload payload, CancellationToken cancellationToken);
        Task<DiaryEntryDtoExercise> GetExerciseAsync(string userId, DateTime date, CancellationToken cancellationToken);
        Task UpsertExerciseAsync(string userId, ExercisePayload payload, CancellationToken cancellationToken);
        Task<bool> IsDiaryFull(string userId, DateTime onDate, CancellationToken cancellationToken);
        Task<ICollection<FavouriteDto>> GetFavouritesAsync(string userId, CancellationToken cancellationToken);
        Task<FavouriteDto> GetFavouriteAsync(string userId, int id, CancellationToken cancellationToken);
        Task<int> InsertFavouriteAsync(string userId, FavouritePayload payload, bool isGeneric, CancellationToken cancellationToken);
        Task<int> UpdateFavouriteAsync(string userId, int id, FavouritePayload payload, CancellationToken cancellationToken);
        Task RemoveFavouriteAsync(string userId, int id, CancellationToken cancellationToken);
        decimal GetProteinsForTargetValue(decimal height, decimal weight, decimal targetValue);
    }
}
