﻿using AutoMapper;
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
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace MyFoodDoc.App.Application.Services
{
    public class DiaryService : IDiaryService
    {
        private static readonly DiaryEntryDtoLiquid _liquidDefault = new DiaryEntryDtoLiquid { Amount = 0 };
        private static readonly DiaryEntryDtoExercise _exerciseDefault = new DiaryEntryDtoExercise { Duration = 0 };

        private readonly IApplicationContext _context;
        private readonly IMapper _mapper;

        public DiaryService(IApplicationContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
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

            var oldIngredients = _context.MealIngredients.Where(x => x.MealId == meal.Id);
            _context.MealIngredients.RemoveRange(oldIngredients);

            if (payload.Ingredients != null)
            {
                var mealIngredients = payload.Ingredients.Select(ingredient => new MealIngredient { MealId = meal.Id, IngredientId = ingredient.Id, Amount = ingredient.Amount });
                _context.MealIngredients.AddRange(mealIngredients);
            }

            await _context.SaveChangesAsync(cancellationToken);

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

            if (payload.Ingredients != null)
            {
                var mealIngredients = payload.Ingredients.Select(ingredient => new MealIngredient { MealId = meal.Id, IngredientId = ingredient.Id, Amount = ingredient.Amount });
                _context.MealIngredients.AddRange(mealIngredients);
            }

            await _context.SaveChangesAsync(cancellationToken);

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
    }
}