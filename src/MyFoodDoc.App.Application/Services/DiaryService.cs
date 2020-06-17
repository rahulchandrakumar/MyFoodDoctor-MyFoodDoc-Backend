using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using MyFoodDoc.App.Application.Abstractions;
using MyFoodDoc.App.Application.Exceptions;
using MyFoodDoc.App.Application.Models;
using MyFoodDoc.App.Application.Payloads.Diary;
using MyFoodDoc.Application.Abstractions;
using MyFoodDoc.Application.Entites;
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

        private readonly IApplicationContext _context;
        private readonly IMapper _mapper;
        private readonly IFatSecretClient _fatSecretClient;
        private readonly int _statisticsPeriod;

        public DiaryService(IApplicationContext context, IMapper mapper, IFatSecretClient fatSecretClient, IOptions<StatisticsOptions> statisticsOptions)
        {
            _context = context;
            _mapper = mapper;
            _fatSecretClient = fatSecretClient;
            _statisticsPeriod = statisticsOptions.Value.Period > 0 ? statisticsOptions.Value.Period : 7;
        }

        public async Task<DiaryEntryDto> GetAggregationByDateAsync(string userId, DateTime start, CancellationToken cancellationToken = default)
        {
            var aggregation = new DiaryEntryDto
            {
                Meals = await _context.Meals
                    .Where(x => x.UserId == userId && x.Date == start)
                    .ProjectTo<DiaryEntryDtoMeal>(_mapper.ConfigurationProvider)
                    .ToListAsync(cancellationToken),
                Liquid = await _context.Liquids
                    .Where(x => x.UserId == userId && x.Date == start)
                    .ProjectTo<DiaryEntryDtoLiquid>(_mapper.ConfigurationProvider)
                    .SingleOrDefaultAsync(cancellationToken) ?? _liquidDefault,
                Exercise = await _context.Exercises
                    .Where(x => x.UserId == userId && x.Date == start)
                    .ProjectTo<DiaryEntryDtoExercise>(_mapper.ConfigurationProvider)
                    .SingleOrDefaultAsync(cancellationToken) ?? _exerciseDefault,
            };

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
            var meal = new Meal
            {
                UserId = userId,
                Date = payload.Date,
                Time = payload.Time,
                Type = payload.Type,
                Mood = payload.Mood,
            };

            _context.Meals.Add(meal);
            
            await _context.SaveChangesAsync(cancellationToken);

            //TODO: Check relevance
            var oldIngredients = _context.MealIngredients.Where(x => x.MealId == meal.Id);

            _context.MealIngredients.RemoveRange(oldIngredients);

            await _context.SaveChangesAsync(cancellationToken);

            await UpsertMealIngredients(meal.Id, payload.Ingredients, cancellationToken);

            return meal.Id;
        }

        public async Task<int> UpdateMealAsync(string userId, int mealId, UpdateMealPayload payload, CancellationToken cancellationToken)
        {
            Meal meal = await _context.Meals
                .Where(x => x.UserId == userId && x.Id == mealId)
                .SingleOrDefaultAsync(cancellationToken);

            meal.Time = payload.Time;
            meal.Type = payload.Type;
            meal.Mood = payload.Mood;

            await _context.SaveChangesAsync(cancellationToken);

            var oldIngredients = _context.MealIngredients.Where(x => x.MealId == mealId);
            
            _context.MealIngredients.RemoveRange(oldIngredients);

            await _context.SaveChangesAsync(cancellationToken);

            await UpsertMealIngredients(meal.Id, payload.Ingredients, cancellationToken);

            return meal.Id;
        }

        public async Task RemoveMealAsync(string userId, int mealId, CancellationToken cancellationToken)
        {
            //TODO: Check and delete unused ingredients

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
            var liquid = await _context.Liquids.SingleOrDefaultAsync(x => x.UserId == userId && x.Date == payload.Date, cancellationToken);

            if (liquid == null)
            {
                liquid = new Liquid
                {
                    UserId = userId,
                    Date = payload.Date,
                };

                _context.Liquids.Add(liquid);
            }

            if (liquid.Amount < payload.Amount)
            {
                liquid.LastAdded = payload.Time;
            }
            liquid.Amount = payload.Amount;

            await _context.SaveChangesAsync(cancellationToken);
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
            var exercise = await _context.Exercises
                .Where(x => x.UserId == userId && x.Date == payload.Date)
                .SingleOrDefaultAsync(cancellationToken);

            if (exercise == null)
            {
                exercise = new Exercise
                {
                    UserId = userId,
                    Date = payload.Date,
                    LastAdded = payload.Time,
                    Duration = payload.Duration,
                };

                _context.Exercises.Add(exercise);
            }

            if (exercise.Duration < payload.Duration)
            {
                exercise.LastAdded = payload.Time;
            }
            exercise.Duration = payload.Duration;

            await _context.SaveChangesAsync(cancellationToken);
        }

        public async Task<bool> IsDiaryFull(string userId, CancellationToken cancellationToken)
        {
            var user = await _context.Users
                .Where(x => x.Id == userId)
                .SingleOrDefaultAsync(cancellationToken);

            if (!user.HasSubscription || user.HasSubscriptionUpdated.Value > DateTime.Now.AddDays(-_statisticsPeriod))
                return false;

            return await _context.Meals
                    .Where(x => x.UserId == userId && x.Date > DateTime.Now.AddDays(-_statisticsPeriod))
                    .Select(x => x.Date)
                    .Distinct()
                    .CountAsync(cancellationToken) > 2;//TODO: Create configuration parameter
        }

        private async Task UpsertMealIngredients(int mealId, IEnumerable<IngredientPayload> ingredients, CancellationToken cancellationToken)
        {
            //TODO: Check and delete unused ingredients

            if (ingredients != null)
            {
                var mealIngredients = new List<MealIngredient>();

                foreach (var ingredient in ingredients)
                {
                    var existingIngredient = _context.Ingredients.SingleOrDefault(x =>
                        x.FoodId == ingredient.FoodId && x.ServingId == ingredient.ServingId);

                    if (existingIngredient == null)
                    {
                        var food = await _fatSecretClient.GetFoodAsync(ingredient.FoodId);

                        if (food == null)
                        {
                            throw new NotFoundException(nameof(Food), ingredient.FoodId);
                        }

                        var serving = food.Servings.Serving.SingleOrDefault(s => s.Id == ingredient.ServingId);

                        if (serving == null)
                        {
                            throw new NotFoundException(nameof(Serving), ingredient.ServingId);
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
                            LastSynchronized = DateTime.Now
                        };

                        await _context.Ingredients.AddAsync(newIngredient);

                        await _context.SaveChangesAsync(cancellationToken);

                        mealIngredients.Add(new MealIngredient { MealId = mealId, IngredientId = newIngredient.Id, Amount = ingredient.Amount });
                    }
                    else
                    {
                        mealIngredients.Add(new MealIngredient { MealId = mealId, IngredientId = existingIngredient.Id, Amount = ingredient.Amount });
                    }
                }

                await _context.MealIngredients.AddRangeAsync(mealIngredients);

                await _context.SaveChangesAsync(cancellationToken);
            }
        }
    }
}
