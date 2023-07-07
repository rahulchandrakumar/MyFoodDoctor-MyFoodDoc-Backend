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
using MyFoodDoc.App.Application.Abstractions.V2;
using MyFoodDoc.App.Application.Models.StatisticsDto;

namespace MyFoodDoc.App.Application.Services
{
    public class DiaryServiceV2 : IDiaryServiceV2
    {
        private readonly IApplicationContext _context;
        private readonly IMapper _mapper;
        private readonly IFatSecretClient _fatSecretClient;
        private readonly IFoodService _foodService;
        private readonly IHtmlPdfService _htmlPdfService;
        private readonly IEmailService _emailService;
        private readonly int _statisticsPeriod;
        private readonly int _statisticsMinimumDays;

        public DiaryServiceV2(
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

        public bool IsDiaryFull(IEnumerable<MealDto> meals, DateTime userCreatedAt, DateTime onDate)
        {
            if (userCreatedAt > onDate.AddDays(-_statisticsPeriod))
                return false;

            return meals
                .Where(x => x.Date > onDate.AddDays(-_statisticsPeriod) && x.Date <= onDate)
                .Select(x => x.Date)
                .Distinct()
                .Count() >= _statisticsMinimumDays;
        }

        public bool IsZPPForbidden(double? userHeight, decimal? weight, bool eatingDisorder)
        {
            if (!userHeight.HasValue)
            {
                return true;
            }

            if (eatingDisorder)
                return true;

            if (weight is null)
                return true;

            var bmiValue = BMI(userHeight.Value, (double) weight.Value);

            return (bmiValue is < 18.5 or > 35);
        }

        public decimal GetCorrectedWeight(decimal height, decimal weight)
        {
            if (BMI((double) height, (double) weight) < 25)
            {
                return weight;
            }

            return height - 100;
        }

        private double BMI(double height, double weight)
        {
            return height == 0 ? 0 : weight / Math.Pow(height / 100, 2);
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

        private async Task UpsertFavouriteIngredients(int favouriteId, IEnumerable<IngredientPayload> ingredients,
            CancellationToken cancellationToken)
        {
            if (ingredients != null)
            {
                var favouriteIngredients = new List<FavouriteIngredient>();

                foreach (var ingredient in ingredients)
                {
                    favouriteIngredients.Add(new FavouriteIngredient
                    {
                        FavouriteId = favouriteId,
                        IngredientId =
                            await UpsertIngredient(ingredient.FoodId, ingredient.ServingId, cancellationToken),
                        Amount = ingredient.Amount
                    });
                }

                await _context.FavouriteIngredients.AddRangeAsync(favouriteIngredients, cancellationToken);

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

        public async Task<ICollection<FavouriteDto>> GetFavouritesAsync(string userId,
            CancellationToken cancellationToken)
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

        public async Task<int> InsertFavouriteAsync(string userId, FavouritePayload payload, bool isGeneric,
            CancellationToken cancellationToken)
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

        public async Task<int> UpdateFavouriteAsync(string userId, int id, FavouritePayload payload,
            CancellationToken cancellationToken)
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

            var data = new DiaryExportModel() {DateFrom = payload.DateFrom, DateTo = payload.DateTo};

            var meals = await _context.Meals
                .Where(x => x.UserId == userId && x.Date >= payload.DateFrom.Date && x.Date <= payload.DateTo.Date)
                .ToListAsync(cancellationToken);

            var liquids = await _context.Liquids
                .Where(x => x.UserId == userId && x.Date >= payload.DateFrom.Date && x.Date <= payload.DateTo.Date)
                .ToListAsync(cancellationToken);

            var exercises = await _context.Exercises
                .Where(x => x.UserId == userId && x.Date >= payload.DateFrom.Date && x.Date <= payload.DateTo.Date)
                .ToListAsync(cancellationToken);

            var dates = meals.Select(x => x.Date).Union(liquids.Select(x => x.Date))
                .Union(exercises.Select(x => x.Date)).OrderBy(x => x);

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

                    var meal = new DiaryExportMealModel {Time = dayMeal.Time, Type = dayMeal.Type};

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

            Stream bodyTemplateStream = Assembly.GetExecutingAssembly()
                .GetManifestResourceStream($"{this.GetType().Namespace}.DiaryExportEmailTemplate.html");

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
                new[]
                {
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