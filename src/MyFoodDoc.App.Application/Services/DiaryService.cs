using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using MyFoodDoc.App.Application.Abstractions;
using MyFoodDoc.App.Application.Exceptions;
using MyFoodDoc.App.Application.Models;
using MyFoodDoc.App.Application.Payloads.Diary;
using MyFoodDoc.Application.Abstractions;
using MyFoodDoc.Application.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MyFoodDoc.FatSecretClient.Abstractions;
using MyFoodDoc.FatSecretClient.Clients;
using MyFoodDoc.App.Application.Configuration;
using Microsoft.Extensions.Options;

namespace MyFoodDoc.App.Application.Services
{
    public class DiaryService : IDiaryService
    {
        private static readonly DiaryEntryDtoLiquid _liquidDefault = new DiaryEntryDtoLiquid { Amount = 0, PredefinedAmount = 0 };
        private static readonly DiaryEntryDtoExercise _exerciseDefault = new DiaryEntryDtoExercise { Duration = 0 };

        private const int SuggestedLiquidAmountPerKilo = 30;

        private readonly IApplicationContext _context;
        private readonly IMapper _mapper;
        private readonly IFatSecretClient _fatSecretClient;
        private readonly int _statisticsPeriod;
        private readonly int _statisticsMinimumDays;

        public DiaryService(IApplicationContext context, IMapper mapper, IFatSecretClient fatSecretClient, IOptions<StatisticsOptions> statisticsOptions)
        {
            _context = context;
            _mapper = mapper;
            _fatSecretClient = fatSecretClient;
            _statisticsPeriod = statisticsOptions.Value.Period > 0 ? statisticsOptions.Value.Period : 7;
            _statisticsMinimumDays = statisticsOptions.Value.MinimumDays > 0 ? statisticsOptions.Value.MinimumDays : 3;
        }

        public async Task<DiaryEntryDto> GetAggregationByDateAsync(string userId, DateTime start, CancellationToken cancellationToken = default)
        {
            var aggregation = new DiaryEntryDto
            {
                Meals = await _context.Meals.AsNoTracking()
                    .Where(x => x.UserId == userId && x.Date == start)
                    .ProjectTo<DiaryEntryDtoMeal>(_mapper.ConfigurationProvider)
                    .ToListAsync(cancellationToken),
                Liquid = await _context.Liquids.AsNoTracking()
                    .Where(x => x.UserId == userId && x.Date == start)
                    .ProjectTo<DiaryEntryDtoLiquid>(_mapper.ConfigurationProvider)
                    .SingleOrDefaultAsync(cancellationToken) ?? _liquidDefault,
                Exercise = await _context.Exercises.AsNoTracking()
                    .Where(x => x.UserId == userId && x.Date == start)
                    .ProjectTo<DiaryEntryDtoExercise>(_mapper.ConfigurationProvider)
                    .SingleOrDefaultAsync(cancellationToken) ?? _exerciseDefault,
            };

            var userWeight = await _context.UserWeights.AsNoTracking()
                .Where(x => x.UserId == userId)
                .OrderBy(x => x.Date).LastOrDefaultAsync(cancellationToken);

            aggregation.Liquid.PredefinedAmount = (int)Math.Round(SuggestedLiquidAmountPerKilo * userWeight.Value);

            return aggregation;
        }

        public async Task<DiaryEntryDtoMeal> GetMealAsync(string userId, int mealId, CancellationToken cancellationToken)
        {
            var meal = await _context.Meals
                .Where(x => x.UserId == userId && x.Id == mealId)
                .ProjectTo<DiaryEntryDtoMeal>(_mapper.ConfigurationProvider)
                .SingleOrDefaultAsync(cancellationToken);

            if (meal == null)
            {
                throw new NotFoundException(nameof(Meal), mealId);
            }

            return meal;
        }

        public async Task<int> InsertMealAsync(string userId, InsertMealPayload payload, CancellationToken cancellationToken)
        {
            await CheckIngredients(payload.Ingredients, cancellationToken);

            var meal = new Meal
            {
                UserId = userId,
                Date = payload.Date,
                Time = payload.Time,
                Type = payload.Type,
                Mood = payload.Mood,
            };

            await _context.Meals.AddAsync(meal, cancellationToken);
            
            await _context.SaveChangesAsync(cancellationToken);

            await UpsertMealIngredients(meal.Id, payload.Ingredients, cancellationToken);

            return meal.Id;
        }

        public async Task<int> UpdateMealAsync(string userId, int mealId, UpdateMealPayload payload, CancellationToken cancellationToken)
        {
            await CheckIngredients(payload.Ingredients, cancellationToken);

            Meal meal = await _context.Meals
                .Where(x => x.UserId == userId && x.Id == mealId)
                .SingleOrDefaultAsync(cancellationToken);

            meal.Time = payload.Time;
            meal.Type = payload.Type;
            meal.Mood = payload.Mood;

            _context.Meals.Update(meal);

            await _context.SaveChangesAsync(cancellationToken);

            var oldIngredients = _context.MealIngredients.Where(x => x.MealId == mealId);
            
            _context.MealIngredients.RemoveRange(oldIngredients);

            await _context.SaveChangesAsync(cancellationToken);

            await UpsertMealIngredients(meal.Id, payload.Ingredients, cancellationToken);

            return meal.Id;
        }

        public async Task RemoveMealAsync(string userId, int mealId, CancellationToken cancellationToken)
        {
            var meal = await _context.Meals
                .Where(x => x.UserId == userId && x.Id == mealId)
                .SingleOrDefaultAsync(cancellationToken);

            _context.Meals.Remove(meal);

            await _context.SaveChangesAsync(cancellationToken);
        }

        public async Task<DiaryEntryDtoLiquid> GetLiquidAsync(string userId, DateTime date, CancellationToken cancellationToken)
        {
            var entity = await _context.Liquids
                .Where(x => x.UserId == userId && x.Date == date)
                .ProjectTo<DiaryEntryDtoLiquid>(_mapper.ConfigurationProvider)
                .SingleOrDefaultAsync(cancellationToken);

            if (entity == null)
            {
                throw new NotFoundException(nameof(Exercise), (userId, date));
            }

            return entity;
        }

        public async Task UpsertLiquidAsync(string userId, LiquidPayload payload, CancellationToken cancellationToken)
        {
            await _context.Liquids
                .Upsert(new Liquid
                {
                    UserId = userId,
                    Date = payload.Date,
                    LastAdded = payload.Time,
                    Amount = payload.Amount
                })
                .On(x => new { x.UserId, x.Date })
                .WhenMatched(x => new Liquid
                {
                    LastAdded = x.Amount < payload.Amount ? payload.Time : x.LastAdded,
                    Amount = payload.Amount
                })
                .RunAsync(cancellationToken);
        }

        public async Task<DiaryEntryDtoExercise> GetExerciseAsync(string userId, DateTime date, CancellationToken cancellationToken)
        {
            var entity = await _context.Exercises
                .Where(x => x.UserId == userId && x.Date == date)
                .ProjectTo<DiaryEntryDtoExercise>(_mapper.ConfigurationProvider)
                .SingleOrDefaultAsync(cancellationToken);

            if (entity == null)
            {
                throw new NotFoundException(nameof(Exercise), (userId, date));
            }

            return entity;
        }

        public async Task UpsertExerciseAsync(string userId, ExercisePayload payload, CancellationToken cancellationToken)
        {
            await _context.Exercises
                .Upsert(new Exercise
                {
                    UserId = userId,
                    Date = payload.Date,
                    LastAdded = payload.Time,
                    Duration = payload.Duration,
                })
                .On(x => new { x.UserId, x.Date })
                .WhenMatched(x => new Exercise
                {
                    LastAdded = x.Duration < payload.Duration ? payload.Time : x.LastAdded,
                    Duration = payload.Duration
                })
                .RunAsync(cancellationToken);
        }

        public async Task<bool> IsDiaryFull(string userId, CancellationToken cancellationToken)
        {
            var user = await _context.Users
                .Where(x => x.Id == userId)
                .SingleOrDefaultAsync(cancellationToken);

            if (user.Created > DateTime.Now.AddDays(-_statisticsPeriod))
                return false;

            return await _context.Meals
                    .Where(x => x.UserId == userId && x.Date > DateTime.Now.AddDays(-_statisticsPeriod))
                    .Select(x => x.Date)
                    .Distinct()
                    .CountAsync(cancellationToken) >= _statisticsMinimumDays;
        }

        private async Task<int> UpsertIngredient(long foodId, long servingId, CancellationToken cancellationToken)
        {
            var existingIngredient = await _context.Ingredients.SingleOrDefaultAsync(x =>
                x.FoodId == foodId && x.ServingId == servingId, cancellationToken);

            if (existingIngredient == null)
            {
                var food = await _fatSecretClient.GetFoodAsync(foodId);

                if (food == null)
                {
                    throw new NotFoundException(nameof(Food), foodId);
                }

                var serving = food.Servings.Serving.SingleOrDefault(s => s.Id == servingId);

                if (serving == null)
                {
                    throw new NotFoundException(nameof(Serving), servingId);
                }

                var newIngredient = new Ingredient
                {
                    FoodId = food.Id,
                    FoodName = food.Name,
                    ServingId = serving.Id,
                    ServingDescription = serving.Description,
                    MetricServingAmount = serving.MetricServingAmount,
                    MetricServingUnit = serving.MetricServingUnit,
                    MeasurementDescription = serving.MeasurementDescription,
                    CaloriesExternal = serving.Calories,
                    CarbohydrateExternal = serving.Carbohydrate,
                    ProteinExternal = serving.Protein,
                    FatExternal = serving.Fat,
                    SaturatedFatExternal = serving.SaturatedFat,
                    PolyunsaturatedFatExternal = serving.PolyunsaturatedFat,
                    MonounsaturatedFatExternal = serving.MonounsaturatedFat,
                    CholesterolExternal = serving.Cholesterol,
                    SodiumExternal = serving.Sodium,
                    PotassiumExternal = serving.Potassium,
                    FiberExternal = serving.Fiber,
                    SugarExternal = serving.Sugar,
                    LastSynchronized = DateTime.Now
                };

                await _context.Ingredients.AddAsync(newIngredient, cancellationToken);

                await _context.SaveChangesAsync(cancellationToken);

                return newIngredient.Id;
            }
            else
            {
                return existingIngredient.Id;
            }
        }
        
        private async Task UpsertMealIngredients(int mealId, IEnumerable<IngredientPayload> ingredients, CancellationToken cancellationToken)
        {
            if (ingredients != null)
            {
                var mealIngredients = new List<MealIngredient>();

                foreach (var ingredient in ingredients)
                {
                    mealIngredients.Add(new MealIngredient { MealId = mealId, IngredientId = await UpsertIngredient(ingredient.FoodId, ingredient.ServingId, cancellationToken), Amount = ingredient.Amount });
                }
                
                await _context.MealIngredients.AddRangeAsync(mealIngredients, cancellationToken);

                await _context.SaveChangesAsync(cancellationToken);
            }
        }

        private async Task CheckIngredients(IEnumerable<IngredientPayload> ingredients,
            CancellationToken cancellationToken)
        {
            if (ingredients != null)
            {
                foreach (var ingredient in ingredients)
                {
                    await UpsertIngredient(ingredient.FoodId, ingredient.ServingId, cancellationToken);
                }
            }
        }
    }
}
