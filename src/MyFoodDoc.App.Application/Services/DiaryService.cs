using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using MyFoodDoc.App.Application.Abstractions;
using MyFoodDoc.App.Application.Exceptions;
using MyFoodDoc.App.Application.Models;
using MyFoodDoc.App.Application.Payloads.Diary;
using MyFoodDoc.Application.Abstractions;
using MyFoodDoc.Application.Configuration;
using MyFoodDoc.Application.Entities;
using MyFoodDoc.Application.Entities.Diary;
using MyFoodDoc.Application.Enums;
using MyFoodDoc.Application.Services;
using MyFoodDoc.FatSecretClient.Abstractions;
using MyFoodDoc.FatSecretClient.Clients;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using MyFoodDoc.Application.EnumEntities;

namespace MyFoodDoc.App.Application.Services
{
    public class DiaryService : IDiaryService
    {
        private static readonly DiaryEntryDtoLiquid _liquidDefault = new DiaryEntryDtoLiquid { Amount = 0, PredefinedAmount = 0 };
        private static readonly DiaryEntryDtoExercise _exerciseDefault = new DiaryEntryDtoExercise { Duration = 0 };

        private const int SuggestedLiquidAmountPerKilo = 30;
        private const int SuggestedVegetables = 500; // in gram
        private const decimal SuggestedFiber = 30; // in gram

        private readonly IApplicationContext _context;
        private readonly IMapper _mapper;
        private readonly IFatSecretClient _fatSecretClient;
        private readonly IFoodService _foodService;
        private readonly IHtmlPdfService _htmlPdfService;
        private readonly IEmailService _emailService;
        private readonly int _statisticsPeriod;
        private readonly int _statisticsMinimumDays;

        public DiaryService(
            IApplicationContext context,
            IMapper mapper,
            IFatSecretClient fatSecretClient,
            IFoodService foodService,
            IHtmlPdfService htmlPdfService,
            IEmailService emailService,
            IOptions<StatisticsOptions> statisticsOptions)
        {
            _context = context;
            _mapper = mapper;
            _fatSecretClient = fatSecretClient;
            _foodService = foodService;
            _htmlPdfService = htmlPdfService;
            _emailService = emailService;
            _statisticsPeriod = statisticsOptions.Value.Period > 0 ? statisticsOptions.Value.Period : 7;
            _statisticsMinimumDays = statisticsOptions.Value.MinimumDays > 0 ? statisticsOptions.Value.MinimumDays : 3;
        }

        public async Task<DiaryEntryDto> GetAggregationByDateAsync(string userId, DateTime start, CancellationToken cancellationToken = default)
        {
            var user = await _context.Users.SingleOrDefaultAsync(x => x.Id == userId, cancellationToken);

            if (user == null)
            {
                throw new NotFoundException(nameof(User), userId);
            }

            var liquid = await _context.Liquids.AsNoTracking()
                    .Where(x => x.UserId == userId && x.Date == start)
                    .ProjectTo<DiaryEntryDtoLiquid>(_mapper.ConfigurationProvider)
                    .SingleOrDefaultAsync(cancellationToken);

            var exercise = await _context.Exercises.AsNoTracking()
                    .Where(x => x.UserId == userId && x.Date == start)
                    .ProjectTo<DiaryEntryDtoExercise>(_mapper.ConfigurationProvider)
                    .SingleOrDefaultAsync(cancellationToken);

            List<DiaryEntryDtoMeal> meals = new List<DiaryEntryDtoMeal>();

            foreach (var meal in await _context.Meals.AsNoTracking().Where(x => x.UserId == userId && x.Date == start).ToListAsync(cancellationToken))
            {
                meals.Add(await GetMealAsync(userId, meal.Id, cancellationToken));
            }

            var aggregation = new DiaryEntryDto
            {
                Meals = meals,
                Liquid = liquid ?? _liquidDefault,
                Exercise = exercise ?? _exerciseDefault,
            };

            var userWeight = await _context.UserWeights.AsNoTracking()
                .Where(x => x.UserId == userId && x.Date <= start)
                .OrderBy(x => x.Date).LastOrDefaultAsync(cancellationToken);

            if (userWeight is null)
            {
                return new DiaryEntryDto();
            }

            aggregation.Liquid.PredefinedAmount = (int)Math.Round(SuggestedLiquidAmountPerKilo * userWeight.Value);

            aggregation.OptimizationAreas = new List<DiaryEntryDtoOptimizationArea>();

            var optimizationAreas = await _context.OptimizationAreas.ToArrayAsync(cancellationToken);

            var sugarOptimizationArea = optimizationAreas.Single(x => x.Type == OptimizationAreaType.Sugar);

            aggregation.OptimizationAreas.Add(new DiaryEntryDtoOptimizationArea() { Key = sugarOptimizationArea.Key, Optimal = sugarOptimizationArea.LineGraphOptimal.Value });

            var proteinOptimizationArea = optimizationAreas.Single(x => x.Type == OptimizationAreaType.Protein);

            var optimalProtein = GetCorrectedWeight(user.Height.Value, userWeight.Value) * proteinOptimizationArea.LineGraphOptimal.Value;

            aggregation.OptimizationAreas.Add(new DiaryEntryDtoOptimizationArea() { Key = proteinOptimizationArea.Key, Optimal = optimalProtein });

            var snackingOptimizationArea = optimizationAreas.Single(x => x.Type == OptimizationAreaType.Snacking);

            aggregation.OptimizationAreas.Add(new DiaryEntryDtoOptimizationArea() { Key = snackingOptimizationArea.Key, Optimal = snackingOptimizationArea.LineGraphOptimal.Value });

            //TODO: check default age
            var optimalCalories = GetCaloriesOptimalValue(userId, user.Birthday == null ? 40 : DateTime.UtcNow.Year - user.Birthday.Value.Year,
                user.Height.Value, userWeight.Value, user.Gender.Value);

            aggregation.OptimizationAreas.Add(new DiaryEntryDtoOptimizationArea() { Key = OptimizationAreaType.Calories.ToString().ToLower(), Optimal = optimalCalories });

            aggregation.OptimizationAreas.Add(new DiaryEntryDtoOptimizationArea() { Key = OptimizationAreaType.Vegetables.ToString().ToLower(), Optimal = SuggestedVegetables });

            aggregation.OptimizationAreas.Add(new DiaryEntryDtoOptimizationArea() { Key = OptimizationAreaType.Fiber.ToString().ToLower(), Optimal = SuggestedFiber });

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

            await UpsertMealFavourites(userId, meal.Id, payload.Favourites, cancellationToken);

            return meal.Id;
        }

        public async Task<int> UpdateMealAsync(string userId, int mealId, UpdateMealPayload payload, CancellationToken cancellationToken)
        {
            await CheckIngredients(payload.Ingredients, cancellationToken);

            Meal meal = await _context.Meals
                .Where(x => x.UserId == userId && x.Id == mealId)
                .SingleOrDefaultAsync(cancellationToken);

            if (meal == null)
            {
                throw new NotFoundException(nameof(Meal), mealId);
            }

            meal.Time = payload.Time;
            meal.Type = payload.Type;
            meal.Mood = payload.Mood;

            _context.Meals.Update(meal);

            await _context.SaveChangesAsync(cancellationToken);

            var oldMealIngredients = await _context.MealIngredients.Where(x => x.MealId == mealId).ToListAsync(cancellationToken);

            if (oldMealIngredients.Any())
            {
                _context.MealIngredients.RemoveRange(oldMealIngredients);

                await _context.SaveChangesAsync(cancellationToken);
            }

            await UpsertMealIngredients(meal.Id, payload.Ingredients, cancellationToken);

            var oldMealFavourites = await _context.MealFavourites
                .Include(x => x.Favourite)
                .Where(x => x.MealId == mealId).ToListAsync(cancellationToken);

            if (oldMealFavourites.Any())
            {
                var favouritesToRemove = oldMealFavourites.
                    Where(x => !payload.Favourites.Any(y => y.Id == x.FavouriteId))
                    .Select(x => x.Favourite).ToList();

                _context.MealFavourites.RemoveRange(oldMealFavourites);

                await _context.SaveChangesAsync(cancellationToken);

                if (favouritesToRemove.Any())
                {
                    _context.Favourites.RemoveRange(favouritesToRemove);

                    await _context.SaveChangesAsync(cancellationToken);
                }
            }

            await UpsertMealFavourites(userId, meal.Id, payload.Favourites, cancellationToken);

            return meal.Id;
        }

        public async Task RemoveMealAsync(string userId, int mealId, CancellationToken cancellationToken)
        {
            var meal = await _context.Meals
                .Where(x => x.UserId == userId && x.Id == mealId)
                .SingleOrDefaultAsync(cancellationToken);

            if (meal == null)
            {
                throw new NotFoundException(nameof(Meal), mealId);
            }

            var favourites = await _context.MealFavourites
                .Include(x => x.Favourite)
                .Where(x => x.MealId == mealId).Select(x => x.Favourite).ToListAsync(cancellationToken);

            _context.Meals.Remove(meal);

            await _context.SaveChangesAsync(cancellationToken);

            if (favourites.Any())
            {
                _context.Favourites.RemoveRange(favourites);

                await _context.SaveChangesAsync(cancellationToken);
            }
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

        public async Task<bool> IsDiaryFull(string userId, DateTime onDate, CancellationToken cancellationToken)
        {
            var user = await _context.Users
                .Where(x => x.Id == userId)
                .SingleOrDefaultAsync(cancellationToken);

            if (user == null)
            {
                throw new NotFoundException(nameof(User), userId);
            }

            var dateToCheck = new DateTime(onDate.Year, onDate.Month, onDate.Day);

            var userCreatedDate = new DateTime(user.Created.Year, user.Created.Month, user.Created.Day);

            if (userCreatedDate > dateToCheck.AddDays(-_statisticsPeriod))
                return false;

            return await _context.Meals
                    .Where(x => x.UserId == userId && x.Date > dateToCheck.AddDays(-_statisticsPeriod) && x.Date <= dateToCheck)
                    .Select(x => x.Date)
                    .Distinct()
                    .CountAsync(cancellationToken) >= _statisticsMinimumDays;
        }

        public async Task<bool> IsZPPForbidden(string userId, DateTime onDate, CancellationToken cancellationToken)
        {
            var user = await _context.Users.FirstOrDefaultAsync(x => x.Id == userId, cancellationToken);

            if (user == null)
            {
                throw new NotFoundException(nameof(User), userId);
            }
            else if(! user.Height.HasValue) 
            {
                return true;
            }

            var eatingDisorder = await _context.UserIndications
                .AnyAsync(x => x.UserId == userId && x.IndicationId == 5);

            if (eatingDisorder)
                return true;

            var diabetes = await _context.UserIndications
                .AnyAsync(x => x.UserId == userId && (x.IndicationId == Indication.DiabetesType1 || x.IndicationId == Indication.DiabetesType2));

            if (diabetes)
                return true;

            var weight = await _context.UserWeights
                .OrderBy(x => x.Date)
                .LastOrDefaultAsync(x => x.UserId == userId && x.Date <= onDate, cancellationToken);

            if (weight is null)
                return true;

            var BMIValue = BMI((double)user.Height.Value, (double)weight.Value);

            if (BMIValue < 18.5 || BMIValue > 35)
                return true;

            return false;
        }

        public decimal GetCorrectedWeight(decimal height, decimal weight)
        {
            if (BMI((double)height, (double)weight) < 25)
            {
                return weight;
            }

            if (height > 100)
                return height - 100;
            
            // Calculate correction factor based on height deviation from 100cm
            var correctionFactor = Math.Max(0, (100 - height) / 5);  // Adjust factor as needed

            // Ensure positive corrected weight
            var correctedWeight = weight - correctionFactor;
            return correctedWeight;
        }

        private decimal GetCaloriesOptimalValue(string userId, int age, decimal height, decimal weight, Gender gender)
        {
            var optimalValue = ((decimal)0.047 * weight + (gender == Gender.Female ? 0 : (decimal)1.009) - (decimal)0.01452 * age + (decimal)3.21) * 239 * (decimal)1.4;

            if (BMI((double)height, (double)weight) < 30)
            {
                //TODO: create enums for Diets, Indications and Motivations
                var adipositas = _context.UserIndications.Where(x => x.UserId == userId).Any(x => x.IndicationId == 2);
                var reduceWeight = _context.UserMotivations.Where(x => x.UserId == userId).Any(x => x.MotivationId == 3);

                if (adipositas || reduceWeight)
                    optimalValue -= 300;
            }
            else
            {
                optimalValue -= 500;
            }

            return optimalValue;
        }

        private double BMI(double height, double weight)
        {
            return height == 0 ? 0 : (double)weight / Math.Pow((double)height / 100, 2);
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

        private async Task UpsertFavouriteIngredients(int favouriteId, IEnumerable<IngredientPayload> ingredients, CancellationToken cancellationToken)
        {
            if (ingredients != null)
            {
                var favouriteIngredients = new List<FavouriteIngredient>();

                foreach (var ingredient in ingredients)
                {
                    favouriteIngredients.Add(new FavouriteIngredient { FavouriteId = favouriteId, IngredientId = await UpsertIngredient(ingredient.FoodId, ingredient.ServingId, cancellationToken), Amount = ingredient.Amount });
                }

                await _context.FavouriteIngredients.AddRangeAsync(favouriteIngredients, cancellationToken);

                await _context.SaveChangesAsync(cancellationToken);
            }
        }

        private async Task UpsertMealFavourites(string userId, int mealId, IEnumerable<MealFavouritePayload> favourites, CancellationToken cancellationToken)
        {
            if (favourites != null)
            {
                var mealFavourites = new List<MealFavourite>();

                foreach (var favourite in favourites)
                {
                    if (favourite.Id == null)
                    {
                        favourite.Id = await InsertFavouriteAsync(userId, favourite, false, cancellationToken);
                    }
                    else
                    {
                        Favourite favouriteToUpdate = await _context.Favourites
                            .Where(x => x.UserId == userId && x.Id == favourite.Id.Value)
                            .SingleOrDefaultAsync(cancellationToken);

                        if (favouriteToUpdate == null)
                        {
                            throw new NotFoundException(nameof(Favourite), favourite.Id);
                        }

                        if (favouriteToUpdate.IsGeneric)
                        {
                            throw new Exception($"A generic favourite with id={favouriteToUpdate.Id} cannot be used in meal");
                        }

                        await UpdateFavouriteAsync(userId, favourite.Id.Value, favourite, cancellationToken);
                    }

                    mealFavourites.Add(new MealFavourite { MealId = mealId, FavouriteId = favourite.Id.Value });
                }

                await _context.MealFavourites.AddRangeAsync(mealFavourites, cancellationToken);

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

        public async Task<ICollection<FavouriteDto>> GetFavouritesAsync(string userId, CancellationToken cancellationToken)
        {
            var favourites = await _context.Favourites
                .Where(x => x.UserId == userId && x.IsGeneric)
                .ProjectTo<FavouriteDto>(_mapper.ConfigurationProvider)
                .ToListAsync(cancellationToken);

            return favourites;
        }

        public async Task<FavouriteDto> GetFavouriteAsync(string userId, int id, CancellationToken cancellationToken)
        {
            var favourite = await _context.Favourites
                .Where(x => x.UserId == userId && x.Id == id)
                .ProjectTo<FavouriteDto>(_mapper.ConfigurationProvider)
                .SingleOrDefaultAsync(cancellationToken);

            if (favourite == null)
            {
                throw new NotFoundException(nameof(Favourite), id);
            }

            return favourite;
        }

        public async Task<int> InsertFavouriteAsync(string userId, FavouritePayload payload, bool isGeneric, CancellationToken cancellationToken)
        {
            await CheckIngredients(payload.Ingredients, cancellationToken);

            var favourite = new Favourite
            {
                UserId = userId,
                Title = payload.Title,
                IsGeneric = isGeneric
            };

            await _context.Favourites.AddAsync(favourite, cancellationToken);

            await _context.SaveChangesAsync(cancellationToken);

            await UpsertFavouriteIngredients(favourite.Id, payload.Ingredients, cancellationToken);

            return favourite.Id;
        }

        public async Task<int> UpdateFavouriteAsync(string userId, int id, FavouritePayload payload, CancellationToken cancellationToken)
        {
            await CheckIngredients(payload.Ingredients, cancellationToken);

            Favourite favourite = await _context.Favourites
                .Where(x => x.UserId == userId && x.Id == id)
                .SingleOrDefaultAsync(cancellationToken);

            if (favourite == null)
            {
                throw new NotFoundException(nameof(Favourite), id);
            }

            favourite.Title = payload.Title;

            _context.Favourites.Update(favourite);

            await _context.SaveChangesAsync(cancellationToken);

            var oldIngredients = _context.FavouriteIngredients.Where(x => x.FavouriteId == id);

            _context.FavouriteIngredients.RemoveRange(oldIngredients);

            await _context.SaveChangesAsync(cancellationToken);

            await UpsertFavouriteIngredients(favourite.Id, payload.Ingredients, cancellationToken);

            return favourite.Id;
        }

        public async Task RemoveFavouriteAsync(string userId, int id, CancellationToken cancellationToken)
        {
            var favourite = await _context.Favourites
                .Where(x => x.UserId == userId && x.Id == id)
                .SingleOrDefaultAsync(cancellationToken);

            if (favourite == null)
            {
                throw new NotFoundException(nameof(Favourite), id);
            }

            _context.Favourites.Remove(favourite);

            await _context.SaveChangesAsync(cancellationToken);
        }

        public async Task ExportAsync(string userId, ExportPayload payload, CancellationToken cancellationToken)
        {
            var user = await _context.Users.SingleOrDefaultAsync(x => x.Id == userId, cancellationToken);

            if (user == null)
            {
                throw new NotFoundException(nameof(User), userId);
            }

            var data = new DiaryExportModel() { DateFrom = payload.DateFrom, DateTo = payload.DateTo };

            var meals = await _context.Meals
                    .Where(x => x.UserId == userId && x.Date >= payload.DateFrom.Date && x.Date <= payload.DateTo.Date).ToListAsync(cancellationToken);

            var liquids = await _context.Liquids
                    .Where(x => x.UserId == userId && x.Date >= payload.DateFrom.Date && x.Date <= payload.DateTo.Date).ToListAsync(cancellationToken);

            var exercises = await _context.Exercises
                   .Where(x => x.UserId == userId && x.Date >= payload.DateFrom.Date && x.Date <= payload.DateTo.Date).ToListAsync(cancellationToken);

            var dates = meals.Select(x => x.Date).Union(liquids.Select(x => x.Date)).Union(exercises.Select(x => x.Date)).OrderBy(x => x);

            data.Days = new List<DiaryExportDayModel>();

            foreach (var date in dates)
            {
                var day = new DiaryExportDayModel
                {
                    Date = date,
                    LiquidAmount = liquids.Where(x => x.Date == date).Sum(y => y.Amount),
                    ExerciseDuration = exercises.Where(x => x.Date == date).Sum(y => y.Duration)
                };

                var dayMeals = meals.Where(x => x.Date == day.Date);

                day.Calories = 0;
                day.Protein = 0;
                day.Sugar = 0;
                day.Vegetables = 0;

                day.Meals = new List<DiaryExportMealModel>();

                foreach (var dayMeal in dayMeals)
                {
                    var mealNutritions = await _foodService.GetMealNutritionsAsync(dayMeal.Id, cancellationToken);

                    day.Calories += mealNutritions.Calories;
                    day.Protein += mealNutritions.Protein;
                    day.Sugar += mealNutritions.Sugar;
                    day.Vegetables += mealNutritions.Vegetables;

                    var meal = new DiaryExportMealModel { Time = dayMeal.Time, Type = dayMeal.Type };

                    meal.Ingredients = new List<DiaryExportMealIngredientModel>();

                    foreach (var mealIngredient in await _context.MealIngredients
                                                    .Include(x => x.Ingredient)
                                                    .Where(x => x.MealId == dayMeal.Id)
                                                    .ToListAsync(cancellationToken))
                        meal.Ingredients.Add(new DiaryExportMealIngredientModel
                        {
                            FoodName = mealIngredient.Ingredient.FoodName,
                            ServingDescription = mealIngredient.Ingredient.ServingDescription,
                            MeasurementDescription = mealIngredient.Ingredient.MeasurementDescription,
                            MetricServingAmount = mealIngredient.Ingredient.MetricServingAmount,
                            MetricServingUnit = mealIngredient.Ingredient.MetricServingUnit,
                            Amount = mealIngredient.Amount,
                        });

                    foreach (var mealFavourite in await _context.MealFavourites
                                                    .Include(x => x.Favourite)
                                                    .ThenInclude(x => x.Ingredients)
                                                    .ThenInclude(x => x.Ingredient)
                                                    .Where(x => x.MealId == dayMeal.Id)
                                                    .ToArrayAsync(cancellationToken))
                    {
                        foreach (var mealFavouriteIngredient in mealFavourite.Favourite.Ingredients)
                        {
                            meal.Ingredients.Add(new DiaryExportMealIngredientModel
                            {
                                FoodName = mealFavouriteIngredient.Ingredient.FoodName,
                                ServingDescription = mealFavouriteIngredient.Ingredient.ServingDescription,
                                MeasurementDescription = mealFavouriteIngredient.Ingredient.MeasurementDescription,
                                MetricServingAmount = mealFavouriteIngredient.Ingredient.MetricServingAmount,
                                MetricServingUnit = mealFavouriteIngredient.Ingredient.MetricServingUnit,
                                Amount = mealFavouriteIngredient.Amount,
                            });
                        }
                    }

                    day.Meals.Add(meal);
                }

                day.Calories = Math.Truncate(day.Calories);
                day.Protein = Math.Truncate(day.Protein);
                day.Sugar = Math.Truncate(day.Sugar);
                day.Vegetables = Math.Truncate(day.Vegetables);

                data.Days.Add(day);
            }

            var bytes = await _htmlPdfService.GenerateExport(data, payload.HtmlStruct);

            Stream bodyTemplateStream = Assembly.GetExecutingAssembly().GetManifestResourceStream($"{this.GetType().Namespace}.DiaryExportEmailTemplate.html");

            if (bodyTemplateStream == null)
            {
                throw new ArgumentNullException(nameof(bodyTemplateStream));
            }

            StreamReader reader = new StreamReader(bodyTemplateStream);
            string body = reader.ReadToEnd();

            var result = await _emailService.SendEmailAsync(
                user.Email,
                null,
                "Tagebuchexport",
                body,
                new[] {
                            new Attachment()
                            {
                                Content = bytes,
                                Filename = "Tagebuch.pdf",
                                Type = "application/pdf"
                            }
                });

            if (!result)
            {
                throw new Exception($"Unable to send an email to {user.Email}");
            }
        }

        public async Task<bool> CheckDiet(string userId, string dietKey, CancellationToken cancellationToken)
        {
            if (userId == null)
            {
                throw new ArgumentNullException(nameof(userId));
            }
            if (dietKey == null)
            {
                throw new ArgumentNullException(nameof(dietKey));
            }

            var query =
                from userDiet in _context.UserDiets
                join diet in _context.Diets on userDiet.DietId equals diet.Id
                where userDiet.UserId == userId && diet.Key == dietKey
                select diet.Id;

            return await query.AnyAsync(cancellationToken);
        }
    }
}
