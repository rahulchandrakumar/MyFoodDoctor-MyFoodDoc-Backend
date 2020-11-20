using MyFoodDoc.App.Application.Models;
using MyFoodDoc.App.Application.Payloads.User;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace MyFoodDoc.App.Application.Abstractions
{
    public interface IUserService
    {
        Task<UserDto> GetUserAsync(string userId, CancellationToken cancellationToken = default);

        Task<UserDto> StoreAnamnesisAsync(string userId, AnamnesisPayload payload, CancellationToken cancellationToken = default);

        Task<UserDto> UpdateUserAsync(string userId, UpdateUserPayload payload, CancellationToken cancellationToken = default);

        Task ChangePasswordAsync(string userId, string oldPassword, string newPassword);

        Task UpdatePushNotifications(string userId, UpdatePushNotificationsPayload payload,
            CancellationToken cancellationToken = default);

        Task<bool> ValidateAppStoreInAppPurchase(string userId, ValidateAppStoreInAppPurchasePayload payload,
            CancellationToken cancellationToken = default);

        Task<bool> ValidateGooglePlayStoreInAppPurchase(string userId, ValidateGooglePlayStoreInAppPurchasePayload payload,
            CancellationToken cancellationToken = default);
    }
}
