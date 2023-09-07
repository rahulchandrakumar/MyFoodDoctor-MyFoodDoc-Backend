using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using MyFoodDoc.App.Application.Abstractions;
using MyFoodDoc.App.Application.Abstractions.V2;
using MyFoodDoc.App.Application.Exceptions;
using MyFoodDoc.App.Application.Extensions;
using MyFoodDoc.App.Application.Models;
using MyFoodDoc.App.Application.Models.StatisticsDto;
using MyFoodDoc.App.Application.Payloads.Diary;
using MyFoodDoc.App.Application.Payloads.User;
using MyFoodDoc.Application.Abstractions;
using MyFoodDoc.Application.Configuration;
using MyFoodDoc.Application.Entities;
using MyFoodDoc.Application.Entities.Subscriptions;
using MyFoodDoc.Application.Enums;
using MyFoodDoc.AppStoreClient.Abstractions;
using MyFoodDoc.GooglePlayStoreClient.Abstractions;

namespace MyFoodDoc.App.Application.Services.V2
{
    public class UserServiceV2 : IUserServiceV2
    {
        private readonly IApplicationContext _context;
        private readonly IMapper _mapper;
        private readonly IUserHistoryService _userHistoryService;
        private readonly UserManager<User> _userManager;
        private readonly IAppStoreClient _appStoreClient;
        private readonly IGooglePlayStoreClient _googlePlayStoreClient;
        private readonly int _statisticsPeriod;

        public UserServiceV2(
            IApplicationContext context,
            IMapper mapper,
            IUserHistoryService userHistoryService,
            UserManager<User> userManager,
            IAppStoreClient appStoreClient,
            IGooglePlayStoreClient googlePlayStoreClient,
            IOptions<StatisticsOptions> statisticsOptions)
        {
            _context = context;
            _mapper = mapper;
            _userManager = userManager;
            _userHistoryService = userHistoryService;
            _appStoreClient = appStoreClient;
            _googlePlayStoreClient = googlePlayStoreClient;
            _statisticsPeriod = statisticsOptions.Value.Period > 0 ? statisticsOptions.Value.Period : 7;
        }

        public async Task<UserDto> GetUserAsync(string userId, CancellationToken cancellationToken = default)
        {
            var result = await _context.Users
                .Include(x => x.Indications)
                .Include(x => x.WeightHistory)
                .ProjectTo<UserDto>(_mapper.ConfigurationProvider)
                .SingleOrDefaultAsync(cancellationToken);

            if (result == null)
            {
                throw new NotFoundException(nameof(User), userId);
            }

            result.HasZPPSubscription = await HasSubscription(userId, SubscriptionType.ZPP, cancellationToken);
            result.HasSubscription = result.HasZPPSubscription ||
                                     await HasSubscription(userId, SubscriptionType.MyFoodDoc, cancellationToken);

            return result;
        }

        

        public async Task<StatisticsUserDto> GetUserWithWeightAsync(string userId,
            CancellationToken cancellationToken = default)
        {
            var result = await _context.Users
                .Where(x => x.Id == userId)
                .AsNoTracking()
                .Select(UserExpressions.Selector(userId))
                .SingleOrDefaultAsync(cancellationToken);
            
            if (result is not null)
            {
                result.Meals = _context.Meals.Where(meal => meal.UserId == userId).Select(m => new MealDto(
                    m.Date,
                    m.Type,
                    m.Ingredients.Where(ingredient => ingredient.MealId == m.Id).Select(mi =>
                        new MealIngredientDto(mi.Ingredient.Protein,
                            mi.Ingredient.ProteinExternal,
                            mi.Ingredient.ContainsPlantProtein,
                            mi.Ingredient.Calories,
                            mi.Ingredient.CaloriesExternal,
                            mi.Ingredient.Sugar,
                            mi.Ingredient.SugarExternal,
                            mi.Ingredient.Vegetables,
                            mi.Amount)),
                    m.Favourites.Where(fav => fav.MealId == m.Id).Select(mf => mf.FavouriteId)
                )).ToList();

                result.UserTargets = _context.UserTargets
                    .Where(targets => targets.UserId == userId)
                    .Select(u =>
                        new UserTargetDto(u.TargetId, u.TargetAnswerCode, u.Created, new FullUserTargetDto(
                            u.TargetId,
                            new OptimizationAreaTargetDto(
                                u.Target.OptimizationArea.ImageId,
                                u.Target.OptimizationArea.Key,
                                u.Target.OptimizationArea.Name,
                                u.Target.OptimizationArea.Text,
                                u.Target.OptimizationArea.LineGraphUpperLimit,
                                u.Target.OptimizationArea.LineGraphLowerLimit,
                                u.Target.OptimizationArea.LineGraphOptimal,
                                u.Target.OptimizationArea.AboveOptimalLineGraphTitle,
                                u.Target.OptimizationArea.AboveOptimalLineGraphText,
                                u.Target.OptimizationArea.BelowOptimalLineGraphTitle,
                                u.Target.OptimizationArea.BelowOptimalLineGraphText,
                                u.Target.OptimizationArea.OptimalLineGraphTitle,
                                u.Target.OptimizationArea.OptimalLineGraphText,
                                u.Target.OptimizationArea.AboveOptimalPieChartTitle,
                                u.Target.OptimizationArea.AboveOptimalPieChartText,
                                u.Target.OptimizationArea.BelowOptimalPieChartTitle,
                                u.Target.OptimizationArea.BelowOptimalPieChartText,
                                u.Target.OptimizationArea.OptimalPieChartTitle,
                                u.Target.OptimizationArea.OptimalPieChartText),
                            u.Target.TriggerOperator,
                            u.Target.TriggerValue,
                            u.Target.Threshold,
                            u.Target.Priority,
                            u.Target.Title,
                            u.Target.Text,
                            u.Target.Type,
                            u.Target.Image.Url,
                            u.Target.AdjustmentTargets.Select(at => new AdjustmentTargetDto(
                                at.TargetId,
                                at.StepDirection,
                                at.Step,
                                at.TargetValue,
                                at.RecommendedText,
                                at.TargetText,
                                at.RemainText)
                            ).FirstOrDefault()
                        ))).ToList();

                var favouriteMealIngredientDtos = await _context.FavouriteIngredients
                    .Where(ind => ind.Favourite.UserId == userId).Select(i =>
                        new
                        {
                            i.FavouriteId,
                            i.Ingredient.Protein,
                            i.Ingredient.ProteinExternal,
                            i.Ingredient.ContainsPlantProtein,
                            i.Ingredient.Calories,
                            i.Ingredient.CaloriesExternal,
                            i.Ingredient.Sugar,
                            i.Ingredient.SugarExternal,
                            i.Ingredient.Vegetables,
                            i.Amount
                        }).ToListAsync(cancellationToken);

                result.FavouriteIngredientDtos = new UserFavouriteDto(
                    favouriteMealIngredientDtos.FirstOrDefault()?.FavouriteId ?? 0,
                    favouriteMealIngredientDtos.Select(i => new FavouriteMealIngredientDto(
                        i.Protein,
                        i.ProteinExternal,
                        i.ContainsPlantProtein,
                        i.Calories,
                        i.CaloriesExternal,
                        i.Sugar,
                        i.SugarExternal,
                        i.Vegetables,
                        i.Amount
                    )).ToList());
            }
            
            return result;
        }

        public async Task<UserDto> StoreAnamnesisAsync(string userId, AnamnesisPayload payload,
            CancellationToken cancellationToken = default)
        {
            var user = await _context.Users.SingleOrDefaultAsync(x => x.Id == userId, cancellationToken);
            if (user == null)
            {
                throw new NotFoundException(nameof(User), userId);
            }

            if (payload.Age.HasValue)
            {
                user.Birthday = DateTime.UtcNow.Date.AddYears(-payload.Age.Value);
            }

            user.Gender = payload.Gender;
            user.Height = payload.Height;

            var oldIndications = await _context.UserIndications.Where(x => x.UserId.Equals(userId))
                .ToListAsync(cancellationToken);
            _context.UserIndications.RemoveRange(oldIndications);

            if (payload.Weight > 0)
            {
                await _userHistoryService.UpsertWeightHistoryAsync(userId,
                    new WeightHistoryPayload {Date = DateTime.UtcNow, Value = payload.Weight}, cancellationToken);
            }

            if (payload.Indications != null)
            {
                var indicationsIds = await _context.Indications
                    .Where(x => payload.Indications.Contains(x.Key))
                    .Select(x => x.Id)
                    .ToListAsync(cancellationToken);

                var userIndications = indicationsIds.Select(indicationId =>
                    new UserIndication {UserId = userId, IndicationId = indicationId});

                await _context.UserIndications.AddRangeAsync(userIndications, cancellationToken);
            }

            var oldMotivations = await _context.UserMotivations.Where(x => x.UserId.Equals(userId))
                .ToListAsync(cancellationToken);
            _context.UserMotivations.RemoveRange(oldMotivations);

            if (payload.Motivations != null)
            {
                var motivationIds = await _context.Motivations
                    .Where(x => payload.Motivations.Contains(x.Key))
                    .Select(x => x.Id)
                    .ToListAsync(cancellationToken);

                var userMotivations = motivationIds.Select(motivationId =>
                    new UserMotivation {UserId = userId, MotivationId = motivationId});

                await _context.UserMotivations.AddRangeAsync(userMotivations, cancellationToken);
            }

            var oldDiets = await _context.UserDiets.Where(x => x.UserId.Equals(userId)).ToListAsync(cancellationToken);
            _context.UserDiets.RemoveRange(oldDiets);

            if (payload.Diets != null)
            {
                var dietIds = await _context.Diets
                    .Where(x => payload.Diets.Contains(x.Key))
                    .Select(x => x.Id)
                    .ToListAsync(cancellationToken);

                var userDiets = dietIds.Select(motivationId => new UserDiet {UserId = userId, DietId = motivationId});

                await _context.UserDiets.AddRangeAsync(userDiets, cancellationToken);
            }

            // TODO: Add weight behavior

            await _context.SaveChangesAsync(cancellationToken);

            return await GetUserAsync(userId, cancellationToken);
        }

        public async Task<UserDto> UpdateUserAsync(string userId, UpdateUserPayload payload,
            CancellationToken cancellationToken = default)
        {
            var user = await _context.Users.SingleOrDefaultAsync(x => x.Id == userId, cancellationToken);
            if (user == null)
            {
                throw new NotFoundException(nameof(User), userId);
            }

            if (payload.Age.HasValue)
            {
                user.Birthday = DateTime.UtcNow.Date.AddYears(-payload.Age.Value);
            }

            user.Gender = payload.Gender;
            user.Height = payload.Height;
            user.InsuranceId = payload.InsuranceId;

            _context.Users.Update(user);

            var oldIndications = await _context.UserIndications.Where(x => x.UserId.Equals(userId))
                .ToListAsync(cancellationToken);

            if (oldIndications.Any())
                _context.UserIndications.RemoveRange(oldIndications);

            if (payload.Indications != null && payload.Indications.Any())
            {
                var indicationsIds = await _context.Indications
                    .Where(x => payload.Indications.Contains(x.Key))
                    .Select(x => x.Id)
                    .ToListAsync(cancellationToken);

                var userIndications = indicationsIds.Select(indicationId =>
                    new UserIndication {UserId = userId, IndicationId = indicationId});

                await _context.UserIndications.AddRangeAsync(userIndications, cancellationToken);
            }

            var oldMotivations = await _context.UserMotivations.Where(x => x.UserId.Equals(userId))
                .ToListAsync(cancellationToken);

            if (oldMotivations.Any())
                _context.UserMotivations.RemoveRange(oldMotivations);

            if (payload.Motivations != null && payload.Motivations.Any())
            {
                var motivationIds = await _context.Motivations
                    .Where(x => payload.Motivations.Contains(x.Key))
                    .Select(x => x.Id)
                    .ToListAsync(cancellationToken);

                var userMotivations = motivationIds.Select(motivationId =>
                    new UserMotivation {UserId = userId, MotivationId = motivationId});

                await _context.UserMotivations.AddRangeAsync(userMotivations, cancellationToken);
            }

            var oldDiets = _context.UserDiets.Where(x => x.UserId.Equals(userId));

            if (oldDiets.Any())
                _context.UserDiets.RemoveRange(oldDiets);

            if (payload.Diets != null && payload.Diets.Any())
            {
                var dietIds = await _context.Diets
                    .Where(x => payload.Diets.Contains(x.Key))
                    .Select(x => x.Id)
                    .ToListAsync(cancellationToken);

                var userDiets = dietIds.Select(dietId => new UserDiet {UserId = userId, DietId = dietId});

                await _context.UserDiets.AddRangeAsync(userDiets, cancellationToken);
            }

            _ = await _context.SaveChangesAsync(cancellationToken);

            return await GetUserAsync(userId, cancellationToken);
        }

        public async Task ChangePasswordAsync(string userId, string oldPassword, string newPassword)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (!await _userManager.CheckPasswordAsync(user, oldPassword))
            {
                throw new BadRequestException("Old password is wrong");
            }

            await _userManager.RemovePasswordAsync(user);
            await _userManager.AddPasswordAsync(user, newPassword);
        }

        public async Task UpdatePushNotifications(string userId, UpdatePushNotificationsPayload payload,
            CancellationToken cancellationToken = default)
        {
            var user = await _context.Users.SingleOrDefaultAsync(x => x.Id == userId, cancellationToken);

            if (user == null)
            {
                throw new NotFoundException(nameof(User), userId);
            }

            var otherUsersWithDeviceToken = await _context.Users
                .Where(x => x.Id != userId && x.DeviceToken == payload.DeviceToken).ToListAsync(cancellationToken);

            foreach (var userToDisablePushNotifications in otherUsersWithDeviceToken)
            {
                userToDisablePushNotifications.PushNotificationsEnabled = false;
                userToDisablePushNotifications.DeviceToken = null;
            }

            user.PushNotificationsEnabled = payload.IsNotificationsEnabled;
            user.DeviceToken = payload.DeviceToken;

            _context.Users.Update(user);

            await _context.SaveChangesAsync(cancellationToken);
        }

        /// <summary>
        ///     This logic is also used in the <see cref="UserExpressions" /> class for performance issue
        ///     Any changes to this method should apply into the "Selector" method for user
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="subscriptionType"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<bool> HasSubscription(string userId, SubscriptionType subscriptionType,
            CancellationToken cancellationToken = default)
        {
            
            var appStoreSubscription =
                await _context.AppStoreSubscriptions.FirstOrDefaultAsync(
                    x => x.UserId == userId && x.Type == subscriptionType && x.IsValid, cancellationToken);

            if (appStoreSubscription != null && appStoreSubscription.IsValid)
                return true;

            var googlePlayStoreSubscription =
                await _context.GooglePlayStoreSubscriptions.FirstOrDefaultAsync(
                    x => x.UserId == userId && x.Type == subscriptionType && x.IsValid, cancellationToken);

            if (googlePlayStoreSubscription != null && googlePlayStoreSubscription.IsValid)
                return true;

            return false;
        }

        public async Task<bool> ValidateAppStoreInAppPurchase(string userId,
            ValidateAppStoreInAppPurchasePayload payload, CancellationToken cancellationToken = default)
        {
            return await ValidateAppStoreInAppPurchase(userId, payload, SubscriptionType.MyFoodDoc, cancellationToken);
        }

        public async Task<bool> ValidateAppStoreZPPSubscription(string userId,
            ValidateAppStoreInAppPurchasePayload payload, CancellationToken cancellationToken = default)
        {
            return await ValidateAppStoreInAppPurchase(userId, payload, SubscriptionType.ZPP, cancellationToken);
        }

        private async Task<bool> ValidateAppStoreInAppPurchase(string userId,
            ValidateAppStoreInAppPurchasePayload payload, SubscriptionType subscriptionType,
            CancellationToken cancellationToken = default)
        {
            var user = await _context.Users.SingleOrDefaultAsync(x => x.Id == userId, cancellationToken);

            if (user == null)
            {
                throw new NotFoundException(nameof(User), userId);
            }

            // NOTE : make sure to save the record, so even if the operation fails, the function can validate it later
            var appStoreSubscription =
                await _context.AppStoreSubscriptions.SingleOrDefaultAsync(
                    x => x.UserId == userId && x.Type == subscriptionType, cancellationToken);
            bool clearIfDoublicate = false;
            if (appStoreSubscription == null)
            {
                clearIfDoublicate = true;
                appStoreSubscription = new AppStoreSubscription
                {
                    UserId = userId,
                    Type = subscriptionType,
                    LastSynchronized = DateTime.Now,
                    ReceiptData = payload.ReceiptData,
                    OriginalTransactionId = "initial",
                    ProductId = "initial",
                    IsValid = false
                };

                _ = await _context.AppStoreSubscriptions.AddAsync(appStoreSubscription, cancellationToken);
                _ = await _context.SaveChangesAsync(cancellationToken);
            }

            var validateReceiptValidationResult =
                await _appStoreClient.ValidateReceipt(subscriptionType, payload.ReceiptData);
            var isValid = subscriptionType == SubscriptionType.MyFoodDoc
                ? validateReceiptValidationResult.SubscriptionExpirationDate > DateTime.Now
                : validateReceiptValidationResult.PurchaseDate.Value.AddYears(1) > DateTime.Now;

            var existingSubscriptions = await _context.AppStoreSubscriptions
                .Where(x => x.ProductId == validateReceiptValidationResult.ProductId && x.OriginalTransactionId ==
                    validateReceiptValidationResult.OriginalTransactionId).ToListAsync(cancellationToken);

            if (existingSubscriptions.Any(x => x.UserId != userId))
            {
                if (clearIfDoublicate)
                {
                    appStoreSubscription.ReceiptData = null;

                    await _context.SaveChangesAsync(cancellationToken);
                }

                throw new ConflictException("Receipt is already used by another user");
            }

            appStoreSubscription.LastSynchronized = DateTime.Now;
            appStoreSubscription.IsValid = isValid;
            appStoreSubscription.ReceiptData = payload.ReceiptData;
            appStoreSubscription.ProductId = validateReceiptValidationResult.ProductId;
            appStoreSubscription.OriginalTransactionId = validateReceiptValidationResult.OriginalTransactionId;


            var googlePlayStoreSubscriptions = await _context.GooglePlayStoreSubscriptions
                .Where(x => x.UserId == userId && x.Type == subscriptionType).ToListAsync(cancellationToken);

            if (googlePlayStoreSubscriptions.Any())
            {
                _context.GooglePlayStoreSubscriptions.RemoveRange(googlePlayStoreSubscriptions);
            }

            await _context.SaveChangesAsync(cancellationToken);

            return appStoreSubscription.IsValid;
        }

        public async Task<bool> ValidateGooglePlayStoreInAppPurchase(string userId,
            ValidateGooglePlayStoreInAppPurchasePayload payload, CancellationToken cancellationToken = default)
        {
            return await ValidateGooglePlayStoreInAppPurchase(userId, payload, SubscriptionType.MyFoodDoc,
                cancellationToken);
        }

        public async Task<bool> ValidateGooglePlayStoreZPPSubscription(string userId,
            ValidateGooglePlayStoreInAppPurchasePayload payload, CancellationToken cancellationToken = default)
        {
            return await ValidateGooglePlayStoreInAppPurchase(userId, payload, SubscriptionType.ZPP, cancellationToken);
        }

        private async Task<bool> ValidateGooglePlayStoreInAppPurchase(string userId,
            ValidateGooglePlayStoreInAppPurchasePayload payload, SubscriptionType subscriptionType,
            CancellationToken cancellationToken = default)
        {
            var user = await _context.Users.SingleOrDefaultAsync(x => x.Id == userId, cancellationToken);

            if (user == null)
            {
                throw new NotFoundException(nameof(User), userId);
            }

            var existingSubscriptions = await _context.GooglePlayStoreSubscriptions
                .Where(x => x.PurchaseToken == payload.PurchaseToken).ToListAsync(cancellationToken);

            if (existingSubscriptions.Any(x => x.UserId != userId))
                throw new ConflictException("PurchaseToken is already used by another user");

            var googlePlayStoreSubscription =
                await _context.GooglePlayStoreSubscriptions.SingleOrDefaultAsync(
                    x => x.UserId == userId && x.Type == subscriptionType, cancellationToken);

            // NOTE : make sure to save the record, so even if the operation fails, the function can validate it later
            if (googlePlayStoreSubscription == null)
            {
                googlePlayStoreSubscription = new GooglePlayStoreSubscription
                {
                    UserId = userId,
                    Type = subscriptionType,
                    LastSynchronized = DateTime.Now,
                    IsValid = false,
                    SubscriptionId = payload.SubscriptionId,
                    PurchaseToken = payload.PurchaseToken
                };

                _ = await _context.GooglePlayStoreSubscriptions.AddAsync(googlePlayStoreSubscription,
                    cancellationToken);
                _ = await _context.SaveChangesAsync(cancellationToken);
            }

            var validateReceiptValidationResult =
                await _googlePlayStoreClient.ValidatePurchase(subscriptionType, payload.SubscriptionId,
                    payload.PurchaseToken);
            var isValid = subscriptionType == SubscriptionType.MyFoodDoc
                ? ((validateReceiptValidationResult.ExpirationDate != null &&
                    validateReceiptValidationResult.ExpirationDate.Value > DateTime.Now &&
                    validateReceiptValidationResult.StartDate != null &&
                    validateReceiptValidationResult.StartDate.Value < DateTime.Now)
                   || (validateReceiptValidationResult.AutoRenewing != null &&
                       validateReceiptValidationResult.AutoRenewing.Value &&
                       validateReceiptValidationResult.ExpirationDate != null &&
                       validateReceiptValidationResult.ExpirationDate.Value < DateTime.Now))
                : validateReceiptValidationResult.PurchaseDate.Value.AddYears(1) > DateTime.Now;

            googlePlayStoreSubscription.LastSynchronized = DateTime.Now;
            googlePlayStoreSubscription.IsValid = isValid;
            googlePlayStoreSubscription.SubscriptionId = payload.SubscriptionId;
            googlePlayStoreSubscription.PurchaseToken = payload.PurchaseToken;

            if (!string.IsNullOrEmpty(validateReceiptValidationResult.LinkedPurchaseToken))
            {
                var linkedGooglePlayStoreSubscription = await _context.GooglePlayStoreSubscriptions.Include(x => x.User)
                    .SingleOrDefaultAsync(x => x.PurchaseToken == validateReceiptValidationResult.LinkedPurchaseToken,
                        cancellationToken);

                if (linkedGooglePlayStoreSubscription != null &&
                    linkedGooglePlayStoreSubscription.User.UserName != user.UserName)
                {
                    linkedGooglePlayStoreSubscription.IsValid = false;

                    _context.GooglePlayStoreSubscriptions.Update(linkedGooglePlayStoreSubscription);
                }
            }

            var appStoreSubscriptions = await _context.AppStoreSubscriptions
                .Where(x => x.UserId == userId && x.Type == subscriptionType).ToListAsync(cancellationToken);

            if (appStoreSubscriptions.Any())
            {
                _context.AppStoreSubscriptions.RemoveRange(appStoreSubscriptions);
            }

            await _context.SaveChangesAsync(cancellationToken);

            return googlePlayStoreSubscription.IsValid;
        }

        public async Task DeleteUserAsync(string userId, CancellationToken cancellationToken = default)
        {
            var user = await _context.Users.SingleOrDefaultAsync(x => x.Id == userId, cancellationToken);

            if (user == null)
            {
                throw new NotFoundException(nameof(User), userId);
            }

            var meals = await _context.Meals
                .Where(x => x.UserId == userId)
                .ToListAsync(cancellationToken);

            if (meals.Any())
                _context.Meals.RemoveRange(meals);

            _context.Users.Remove(user);

            await _context.SaveChangesAsync(cancellationToken);
        }
    }
}