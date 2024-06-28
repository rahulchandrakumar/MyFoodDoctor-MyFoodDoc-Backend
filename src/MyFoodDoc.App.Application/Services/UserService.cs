using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using MyFoodDoc.App.Application.Abstractions;
using MyFoodDoc.App.Application.Exceptions;
using MyFoodDoc.App.Application.Models;
using MyFoodDoc.App.Application.Payloads.Diary;
using MyFoodDoc.App.Application.Payloads.User;
using MyFoodDoc.Application.Abstractions;
using MyFoodDoc.Application.Entities;
using MyFoodDoc.Application.Entities.Subscriptions;
using MyFoodDoc.Application.Enums;
using MyFoodDoc.AppStoreClient.Abstractions;
using MyFoodDoc.GooglePlayStoreClient.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace MyFoodDoc.App.Application.Services
{
    public class UserService : IUserService
    {
        private readonly IApplicationContext _context;
        private readonly IMapper _mapper;
        private readonly IUserHistoryService _userHistoryService;
        private readonly UserManager<User> _userManager;
        private readonly IAppStoreClient _appStoreClient;
        private readonly IGooglePlayStoreClient _googlePlayStoreClient;

        public UserService(
            IApplicationContext context,
            IMapper mapper,
            IUserHistoryService userHistoryService,
            UserManager<User> userManager,
            IAppStoreClient appStoreClient,
            IGooglePlayStoreClient googlePlayStoreClient)
        {
            _context = context;
            _mapper = mapper;
            _userManager = userManager;
            _userHistoryService = userHistoryService;
            _appStoreClient = appStoreClient;
            _googlePlayStoreClient = googlePlayStoreClient;
        }

        public async Task<UserDto> GetUserAsync(string userId, CancellationToken cancellationToken = default)
        {
            var result = await _context.Users
                .Where(x => x.Id == userId)
                .ProjectTo<UserDto>(_mapper.ConfigurationProvider)
                .SingleOrDefaultAsync(cancellationToken);

            if (result == null)
            {
                throw new NotFoundException(nameof(User), userId);
            }

            result.HasZPPSubscription = await HasSubscription(userId, SubscriptionType.ZPP, cancellationToken);
            result.HasSubscription = result.HasZPPSubscription ? true : await HasSubscription(userId, SubscriptionType.MyFoodDoc, cancellationToken);

            return result;
        }

        public async Task<UserDto> StoreAnamnesisAsync(string userId, AnamnesisPayload payload, CancellationToken cancellationToken = default)
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

            var oldIndications = await _context.UserIndications.Where(x => x.UserId.Equals(userId)).ToListAsync(cancellationToken);
            _context.UserIndications.RemoveRange(oldIndications);

            if (payload.Weight > 0)
            {
                await _userHistoryService.UpsertWeightHistoryAsync(userId,
                    new WeightHistoryPayload { Date = DateTime.UtcNow, Value = payload.Weight }, cancellationToken);
            }

            if (payload.Indications != null)
            {
                var indicationsIds = await _context.Indications
                    .Where(x => payload.Indications.ToArray().Contains(x.Key))
                    .Select(x => x.Id)
                    .ToListAsync(cancellationToken);

                var userIndications = indicationsIds.Select(indicationId => new UserIndication { UserId = userId, IndicationId = indicationId });

                await _context.UserIndications.AddRangeAsync(userIndications, cancellationToken);
            }

            var oldMotivations = await _context.UserMotivations.Where(x => x.UserId.Equals(userId)).ToListAsync(cancellationToken);
            _context.UserMotivations.RemoveRange(oldMotivations);

            if (payload.Motivations != null)
            {
                var motivationIds = await _context.Motivations
                    .Where(x => payload.Motivations.ToArray().Contains(x.Key))
                    .Select(x => x.Id)
                    .ToListAsync(cancellationToken);

                var userMotivations = motivationIds.Select(motivationId => new UserMotivation { UserId = userId, MotivationId = motivationId });

                await _context.UserMotivations.AddRangeAsync(userMotivations, cancellationToken);
            }

            var oldDiets = await _context.UserDiets.Where(x => x.UserId.Equals(userId)).ToListAsync(cancellationToken);
            _context.UserDiets.RemoveRange(oldDiets);

            if (payload.Diets != null)
            {
                var dietIds = await _context.Diets
                    .Where(x => payload.Diets.ToArray().Contains(x.Key))
                    .Select(x => x.Id)
                    .ToListAsync(cancellationToken);

                var userDiets = dietIds.Select(motivationId => new UserDiet { UserId = userId, DietId = motivationId });

                await _context.UserDiets.AddRangeAsync(userDiets, cancellationToken);
            }

            // TODO: Add weight behavior

            await _context.SaveChangesAsync(cancellationToken);

            //var indications = await GetIndicationsAsync(userId, cancellationToken);
            //var motivations = await GetMotivationsAsync(userId, cancellationToken);
            //var diets = await GetDietsAsync(userId, cancellationToken);

            return await GetUserAsync(userId, cancellationToken);
        }

        public async Task<UserDto> UpdateUserAsync(string userId, UpdateUserPayload payload, CancellationToken cancellationToken = default)
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
            user.HasZPPVersion = payload.HasZPPVersion;

            _context.Users.Update(user);

            var oldIndications = await _context.UserIndications.Where(x => x.UserId.Equals(userId)).ToListAsync(cancellationToken);

            if (oldIndications.Any())
                _context.UserIndications.RemoveRange(oldIndications);

            if (payload.Indications != null && payload.Indications.Any())
            {
                var indicationsIds = await _context.Indications
                    .Where(x => payload.Indications.ToArray().Contains(x.Key))
                    .Select(x => x.Id)
                    .ToListAsync(cancellationToken);

                var userIndications = indicationsIds.Select(indicationId => new UserIndication { UserId = userId, IndicationId = indicationId });

                await _context.UserIndications.AddRangeAsync(userIndications, cancellationToken);
            }

            var oldMotivations = await _context.UserMotivations.Where(x => x.UserId.Equals(userId)).ToListAsync(cancellationToken);

            if (oldMotivations.Any())
                _context.UserMotivations.RemoveRange(oldMotivations);

            if (payload.Motivations != null && payload.Motivations.Any())
            {
                var motivationIds = await _context.Motivations
                    .Where(x => payload.Motivations.ToArray().Contains(x.Key))
                    .Select(x => x.Id)
                    .ToListAsync(cancellationToken);

                var userMotivations = motivationIds.Select(motivationId => new UserMotivation { UserId = userId, MotivationId = motivationId });

                await _context.UserMotivations.AddRangeAsync(userMotivations, cancellationToken);
            }

            var oldDiets = _context.UserDiets.Where(x => x.UserId.Equals(userId));

            if (oldDiets.Any())
                _context.UserDiets.RemoveRange(oldDiets);

            if (payload.Diets != null && payload.Diets.Any())
            {
                var dietIds = await _context.Diets
                    .Where(x => payload.Diets.ToArray().Contains(x.Key))
                    .Select(x => x.Id)
                    .ToListAsync(cancellationToken);

                var userDiets = dietIds.Select(dietId => new UserDiet { UserId = userId, DietId = dietId });

                await _context.UserDiets.AddRangeAsync(userDiets, cancellationToken);
            }

            _ = await _context.SaveChangesAsync(cancellationToken);

            return await GetUserAsync(userId, cancellationToken);
        }

        private async Task<IList<string>> GetIndicationsAsync(string userId, CancellationToken cancellationToken = default)
        {
            if (userId == null)
            {
                throw new ArgumentNullException(nameof(userId));
            }

            var query =
                from userIndication in _context.UserIndications
                join indication in _context.Indications on userIndication.IndicationId equals indication.Id
                where userIndication.UserId == userId
                select indication.Key;

            return await query.ToListAsync(cancellationToken);
        }

        private async Task<IList<string>> GetMotivationsAsync(string userId, CancellationToken cancellationToken = default)
        {
            if (userId == null)
            {
                throw new ArgumentNullException(nameof(userId));
            }

            var query =
                from userMotivation in _context.UserMotivations
                join motivation in _context.Motivations on userMotivation.MotivationId equals motivation.Id
                where userMotivation.UserId == userId
                select motivation.Key;

            return await query.ToListAsync(cancellationToken);
        }

        private async Task<IList<string>> GetDietsAsync(string userId, CancellationToken cancellationToken = default)
        {
            if (userId == null)
            {
                throw new ArgumentNullException(nameof(userId));
            }

            var query =
                from userDiet in _context.UserDiets
                join diet in _context.Diets on userDiet.DietId equals diet.Id
                where userDiet.UserId == userId
                select diet.Key;

            return await query.ToListAsync(cancellationToken);
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

            var otherUsersWithDeviceToken = await _context.Users.Where(x => x.Id != userId && x.DeviceToken == payload.DeviceToken).ToListAsync(cancellationToken);

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

        public async Task<bool> HasSubscription(string userId, SubscriptionType subscriptionType, CancellationToken cancellationToken = default)
        {
            var appStoreSubscription = await _context.AppStoreSubscriptions.FirstOrDefaultAsync(x => x.UserId == userId && x.Type == subscriptionType && x.IsValid, cancellationToken);

            if (appStoreSubscription != null && appStoreSubscription.IsValid)
                return true;

            var googlePlayStoreSubscription = await _context.GooglePlayStoreSubscriptions.FirstOrDefaultAsync(x => x.UserId == userId && x.Type == subscriptionType && x.IsValid, cancellationToken);

            if (googlePlayStoreSubscription != null && googlePlayStoreSubscription.IsValid)
                return true;

            return false;
        }

        public async Task<bool> HasAnySubscription(string userId, SubscriptionType[] subscriptionTypes, CancellationToken cancellationToken = default)
        {
            return await _context.AppStoreSubscriptions
                .Where(x => x.UserId == userId && subscriptionTypes.Contains(x.Type) && x.IsValid)
                .Select(x => x.Type)
                .Union(_context.GooglePlayStoreSubscriptions
                    .Where(x => x.UserId == userId && subscriptionTypes.Contains(x.Type) && x.IsValid)
                    .Select(x => x.Type))
                .AnyAsync(cancellationToken);
        }

        public async Task<bool> ValidateAppStoreInAppPurchase(string userId, ValidateAppStoreInAppPurchasePayload payload, CancellationToken cancellationToken = default)
        {
            return await ValidateAppStoreInAppPurchase(userId, payload, SubscriptionType.MyFoodDoc, cancellationToken);
        }

        public async Task<bool> ValidateAppStoreZPPSubscription(string userId, ValidateAppStoreInAppPurchasePayload payload, CancellationToken cancellationToken = default)
        {
            return await ValidateAppStoreInAppPurchase(userId, payload, SubscriptionType.ZPP, cancellationToken);
        }

        private async Task<bool> ValidateAppStoreInAppPurchase(string userId, ValidateAppStoreInAppPurchasePayload payload, SubscriptionType subscriptionType, CancellationToken cancellationToken = default)
        {
            var user = await _context.Users.SingleOrDefaultAsync(x => x.Id == userId, cancellationToken);

            if (user == null)
            {
                throw new NotFoundException(nameof(User), userId);
            }

            // NOTE : make sure to save the record, so even if the operation fails, the function can validate it later
            var appStoreSubscription = await _context.AppStoreSubscriptions.SingleOrDefaultAsync(x => x.UserId == userId && x.Type == subscriptionType, cancellationToken);
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

            var validateReceiptValidationResult = await _appStoreClient.ValidateReceipt(subscriptionType, payload.ReceiptData);
            var isValid = subscriptionType == SubscriptionType.MyFoodDoc ? validateReceiptValidationResult.SubscriptionExpirationDate > DateTime.Now : validateReceiptValidationResult.PurchaseDate.Value.AddYears(1) > DateTime.Now;

            var existingSubscriptions = await _context.AppStoreSubscriptions.Where(x => x.ProductId == validateReceiptValidationResult.ProductId && x.OriginalTransactionId == validateReceiptValidationResult.OriginalTransactionId).ToListAsync(cancellationToken);

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

            //_ = _context.AppStoreSubscriptions.Update(appStoreSubscription);

            var googlePlayStoreSubscriptions = await _context.GooglePlayStoreSubscriptions.Where(x => x.UserId == userId && x.Type == subscriptionType).ToListAsync(cancellationToken);

            if (googlePlayStoreSubscriptions.Any())
            {
                _context.GooglePlayStoreSubscriptions.RemoveRange(googlePlayStoreSubscriptions);
            }

            await _context.SaveChangesAsync(cancellationToken);

            return appStoreSubscription.IsValid;
        }

        public async Task<bool> ValidateGooglePlayStoreInAppPurchase(string userId, ValidateGooglePlayStoreInAppPurchasePayload payload, CancellationToken cancellationToken = default)
        {
            return await ValidateGooglePlayStoreInAppPurchase(userId, payload, SubscriptionType.MyFoodDoc, cancellationToken);
        }

        public async Task<bool> ValidateGooglePlayStoreZPPSubscription(string userId, ValidateGooglePlayStoreInAppPurchasePayload payload, CancellationToken cancellationToken = default)
        {
            return await ValidateGooglePlayStoreInAppPurchase(userId, payload, SubscriptionType.ZPP, cancellationToken);
        }

        private async Task<bool> ValidateGooglePlayStoreInAppPurchase(string userId, ValidateGooglePlayStoreInAppPurchasePayload payload, SubscriptionType subscriptionType, CancellationToken cancellationToken = default)
        {
            var user = await _context.Users.SingleOrDefaultAsync(x => x.Id == userId, cancellationToken);

            if (user == null)
            {
                throw new NotFoundException(nameof(User), userId);
            }

            var existingSubscriptions = await _context.GooglePlayStoreSubscriptions.Where(x => x.PurchaseToken == payload.PurchaseToken).ToListAsync(cancellationToken);

            if (existingSubscriptions.Any(x => x.UserId != userId))
                throw new ConflictException("PurchaseToken is already used by another user");

            var googlePlayStoreSubscription = await _context.GooglePlayStoreSubscriptions.SingleOrDefaultAsync(x => x.UserId == userId && x.Type == subscriptionType, cancellationToken);

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

                _ = await _context.GooglePlayStoreSubscriptions.AddAsync(googlePlayStoreSubscription, cancellationToken);
                _ = await _context.SaveChangesAsync(cancellationToken);
            }

            var validateReceiptValidationResult = await _googlePlayStoreClient.ValidatePurchase(subscriptionType, payload.SubscriptionId, payload.PurchaseToken);
            var isValid = subscriptionType == SubscriptionType.MyFoodDoc ? ((validateReceiptValidationResult.ExpirationDate != null && validateReceiptValidationResult.ExpirationDate.Value > DateTime.Now &&
                                          validateReceiptValidationResult.StartDate != null && validateReceiptValidationResult.StartDate.Value < DateTime.Now)
                                         || (validateReceiptValidationResult.AutoRenewing != null && validateReceiptValidationResult.AutoRenewing.Value &&
                                             validateReceiptValidationResult.ExpirationDate != null && validateReceiptValidationResult.ExpirationDate.Value < DateTime.Now)) : validateReceiptValidationResult.PurchaseDate.Value.AddYears(1) > DateTime.Now;

            googlePlayStoreSubscription.LastSynchronized = DateTime.Now;
            googlePlayStoreSubscription.IsValid = isValid;
            googlePlayStoreSubscription.SubscriptionId = payload.SubscriptionId;
            googlePlayStoreSubscription.PurchaseToken = payload.PurchaseToken;

            //_context.GooglePlayStoreSubscriptions.Update(googlePlayStoreSubscription);

            if (!string.IsNullOrEmpty(validateReceiptValidationResult.LinkedPurchaseToken))
            {
                var linkedGooglePlayStoreSubscription = await _context.GooglePlayStoreSubscriptions.Include(x => x.User).SingleOrDefaultAsync(x => x.PurchaseToken == validateReceiptValidationResult.LinkedPurchaseToken, cancellationToken);

                if (linkedGooglePlayStoreSubscription != null && linkedGooglePlayStoreSubscription.User.UserName != user.UserName)
                {
                    linkedGooglePlayStoreSubscription.IsValid = false;

                    _context.GooglePlayStoreSubscriptions.Update(linkedGooglePlayStoreSubscription);
                }
            }

            var appStoreSubscriptions = await _context.AppStoreSubscriptions.Where(x => x.UserId == userId && x.Type == subscriptionType).ToListAsync(cancellationToken);

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
